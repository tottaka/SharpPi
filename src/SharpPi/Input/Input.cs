using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Numerics;

/// <summary>
/// Raw Input library for the raspberry pi on linux os.
/// https://www.kernel.org/doc/Documentation/input/input.txt
/// </summary>
namespace SharpPi.Input
{
    public enum InputDeviceType { ANY = 0, KEYBOARD = 1, MOUSE = 2 };
    public enum MouseButton : byte { NONE = 8, LEFT = 9, RIGHT = 10, SCROLL = 12 };
    public enum InputState : byte { NONE = 0, DOWN = 1, UP = 2 };

    public struct EventTimeData
    {
        public const int Size = 16;
    }

    /// <summary>
    /// 'time' is the timestamp, it returns the time at which the event happened.
    /// Type is for example EV_REL for relative moment, EV_KEY for a keypress or
    /// release. More types are defined in include/uapi/linux/input-event-codes.h.
    ///
    /// 'code' is event code, for example REL_X or KEY_BACKSPACE, again a complete
    /// list is in include/uapi/linux/input-event-codes.h.
    ///
    /// 'value' is the value the event carries. Either a relative change for
    /// EV_REL, absolute new value for EV_ABS(joysticks...), or 0 for EV_KEY for
    /// release, 1 for keypress and 2 for autorepeat.
    /// </summary>
    public struct KeyboardInputEventData
    {
        public const int Size = EventTimeData.Size + (sizeof(ushort) * 2) + sizeof(uint);

        public EventTimeData Time;
        public ushort Type;
        public ushort Code;
        public uint Value;

        public override string ToString() => string.Format("KeyboardInputEventData: {0}, {1}, {2}", Type, Code, Value);
    };

    public struct MouseInputEventData
    {
        /// <summary>
        /// Will be 3 bytes for PS/2 mice.
        /// </summary>
        public const int Size = 3;

        public MouseButton Button;
        public Vector2 Position;

        public override string ToString() => string.Format("MouseInputEventData: {0}, {1}", Button, Position);
    }

    public struct InputDeviceDetails
    {
        /// <summary>
        /// Id of the device (struct input_id)
        /// https://www.kernel.org/doc/Documentation/input/input.txt
        /// </summary>
        public string Id;

        /// <summary>
        /// Name of the device.
        /// </summary>
        public string Name;

        /// <summary>
        /// Physical path to the device in the system hierarchy.
        /// </summary>
        public string Path;

        /// <summary>
        /// Sysfs path
        /// </summary>
        public string SysPath;

        /// <summary>
        /// Unique identification code for the device (if device has it)
        /// </summary>
        public string Uid;

        /// <summary>
        /// The input handles associated with the device.
        /// </summary>
        public string[] Handles;

        public Dictionary<string, string> Bitmaps;

        public string[] GetEvents() => Handles.Where(t => t.StartsWith("event")).ToArray();

        public static InputDeviceDetails Empty = new InputDeviceDetails { Id = string.Empty, Uid = string.Empty, Name = string.Empty, Path = string.Empty, SysPath = string.Empty, Handles = new string[0], Bitmaps = new Dictionary<string, string>() };
    }

    // we can differentiate each device by the Phys=usb-3f980000.usb-1.1.3/input1 where the 1.1.3 is the usb port the device is plugged into
    // not sure if this would still work with usb hubs though... but for now it should work
    // an easier way by just reading from the /dev/input/by-id or /dev/input/by-path
    // this should also just be split into two classes like Cursor and Keyboard, instead of one class that combines the two.
    public static class Input
    {
        // an easier way by just reading from the /dev/input/by-id or /dev/input/by-path
        public const string KeyboardDevPath = "/dev/input";
        private const string KeyboardHandleString = "kbd";

        public static bool Initialized { get; private set; }
        public static Vector2 CursorPosition { get; private set; }

        /// <summary>
        /// The <see cref="InputDeviceDetails"/> list from the most recent call to the <see cref="GetInputDevices"/> method.
        /// </summary>
        public static InputDeviceDetails[] InputDeviceDetails { get; private set; }

        /// <summary>
        /// This event is called when a new input device has been plugged in.
        /// </summary>
        public static event EventHandler<InputDeviceDetails> OnDevicePluggedIn;

        /// <summary>
        /// This event is called when a new input device has been plugged in.
        /// </summary>
        public static event EventHandler<InputDeviceDetails> OnDeviceUnplugged;

        public static event EventHandler<MouseInputEventData> OnMouseInputEvent;

        private static Dictionary<string, InputWatcherTask> InputWatcherTasks = new Dictionary<string, InputWatcherTask>();
        private static FileSystemWatcher InputDeviceWatcher;

        private static readonly object ThreadLock = new object();
        private static Vector2 CursorBounds;

        /// <summary>
        /// Initialize the input event processor and starts raising input events.
        /// </summary>
        public static void Initialize(Vector2 bounds)
        {
            lock (ThreadLock)
            {
                if (!Initialized)
                {
                    SetCursorBounds(bounds);
                    InputDeviceWatcher = new FileSystemWatcher(KeyboardDevPath, "event*") { EnableRaisingEvents = true };
                    InputDeviceWatcher.Created += InputDeviceWatcher_Created;
                    InputDeviceWatcher.Deleted += InputDeviceWatcher_Deleted;

                    // initialize mouse watcher all the time
                    Watch("mice", InternalMouseInputEvent);

                    Initialized = true;
                }
            }
        }

        public static void Shutdown()
        {
            lock (ThreadLock)
            {
                if (Initialized)
                {
                    InputDeviceWatcher?.Dispose();
                    foreach (var watcherTask in InputWatcherTasks)
                        watcherTask.Value.Cancel();
                    InputWatcherTasks.Clear();

                    Initialized = false;
                }
            }
        }

        public static void SetCursorBounds(Vector2 bounds)
        {
            CursorBounds = bounds;
        }

        public static void Watch(string inputEvent, EventHandler<MouseInputEventData> callback)
        {
            lock (ThreadLock)
            {
                if (InputWatcherTasks.ContainsKey(inputEvent))
                    throw new InvalidOperationException("The input event watcher for '" + inputEvent + "' already exists!");
                InputWatcherTasks.Add(inputEvent, new InputWatcherTask(inputEvent, callback));
            }
        }

        // we need to do some filtering so we only call the events once per device instead of like 3 times for 1 keyboard
        private static void InputDeviceWatcher_Created(object sender, FileSystemEventArgs e)
        {
            GetInputDevices();
            OnDevicePluggedIn?.Invoke(sender, InputDeviceDetails.Where(t => t.Handles.Contains(e.Name)).First());
        }

        private static void InputDeviceWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (InputWatcherTasks.ContainsKey(e.Name))
            {
                // cancel and remove this element
                InputWatcherTasks[e.Name].Cancel();
                InputWatcherTasks.Remove(e.Name);
            }

            OnDeviceUnplugged?.Invoke(sender, InputDeviceDetails.Where(t => t.Handles.Contains(e.Name)).FirstOrDefault());
            GetInputDevices();
        }

        private static void InternalMouseInputEvent(object sender, MouseInputEventData eventData)
        {
            CursorPosition = Vector2.Clamp(CursorPosition + eventData.Position, Vector2.Zero, CursorBounds);
            OnMouseInputEvent?.Invoke(sender, eventData);
        }

        public static InputDeviceDetails[] GetInputDevices(InputDeviceType type = InputDeviceType.ANY)
        {
            lock (ThreadLock)
            {
                List<InputDeviceDetails> InputDevices = new List<InputDeviceDetails>();
                string[] device_output = File.ReadAllLines("/proc/bus/input/devices");

                InputDeviceDetails CurrentDeviceDetails = new InputDeviceDetails { Bitmaps = new Dictionary<string, string>() };
                foreach (string line in device_output)
                {
                    string output = line.Trim();

                    if (string.IsNullOrWhiteSpace(output))
                    {
                        if (type == InputDeviceType.KEYBOARD && !CurrentDeviceDetails.Handles.Contains(KeyboardHandleString))
                            continue;
                        else if (type == InputDeviceType.MOUSE && !CurrentDeviceDetails.Handles.Contains(KeyboardHandleString)) // need to change this for mice
                            continue;

                        InputDevices.Add(CurrentDeviceDetails);
                        CurrentDeviceDetails = new InputDeviceDetails();
                        CurrentDeviceDetails.Bitmaps = new Dictionary<string, string>();
                        continue;
                    }

                    if (output.StartsWith("I"))
                    {
                        CurrentDeviceDetails.Id = output.Remove(0, 3);
                    }
                    else if (output.StartsWith("N"))
                    {
                        CurrentDeviceDetails.Name = output.Remove(0, 8).Trim('"');
                    }
                    else if (output.StartsWith("P"))
                    {
                        CurrentDeviceDetails.Path = output.Remove(0, 8);
                    }
                    else if (output.StartsWith("S"))
                    {
                        CurrentDeviceDetails.SysPath = output.Remove(0, 9);
                    }
                    else if (output.StartsWith("U"))
                    {
                        CurrentDeviceDetails.Uid = output.Remove(0, 8);
                    }
                    else if (output.StartsWith("H"))
                    {
                        CurrentDeviceDetails.Handles = output.Remove(0, 12).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else if (output.StartsWith("B"))
                    {
                        string bitmap = output.Remove(0, 3);
                        string[] kvPair = bitmap.Split(new char[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
                        if (CurrentDeviceDetails.Bitmaps.ContainsKey(kvPair[0]))
                            CurrentDeviceDetails.Bitmaps[kvPair[0]] = kvPair[1];
                        else CurrentDeviceDetails.Bitmaps.Add(kvPair[0], kvPair[1]);
                    }
                }

                return InputDeviceDetails = InputDevices.ToArray();
            }
        }

        /// <summary>
        /// Make this more generic?x
        /// </summary>
        internal class InputWatcherTask : IDisposable
        {
            public bool IsDisposed { get; private set; }
            public bool IsCanceled { get; private set; }

            private CancellationTokenSource Token;
            private Task WatchTask;

            public event EventHandler<MouseInputEventData> OnMouseInputData;

            private readonly object ThreadLock = new object();

            public InputWatcherTask(string inputEvent, EventHandler<MouseInputEventData> eventCallback)
            {
                OnMouseInputData = eventCallback;
                Token = new CancellationTokenSource();
                WatchTask = Watch(inputEvent, OnWatchTaskEventCallback, Token.Token).ContinueWith(OnWatchTaskCanceled);
            }

            ~InputWatcherTask()
            {
                Dispose();
            }

            public void Cancel()
            {
                lock (ThreadLock)
                {
                    if (!IsCanceled)
                    {
                        Token.Cancel();
                        WatchTask.Wait();
                    }
                }
            }

            private void OnWatchTaskEventCallback(MouseInputEventData eventData) => OnMouseInputData?.Invoke(this, eventData);
            private void OnWatchTaskCanceled(Task task)
            {
                if (!task.IsCanceled)
                    throw task.Exception?.InnerException;
                task.Dispose();
                IsCanceled = true;
            }

            public void Dispose()
            {
                lock (ThreadLock)
                {
                    if (!IsDisposed)
                    {
                        if (!IsCanceled)
                            Cancel();
                        Token.Dispose();
                        WatchTask.Dispose();
                        IsDisposed = true;
                    }
                }
            }

            /// <summary>
            /// keyboard = event0, mouse = /dev/input/mice
            /// should not have to open all of them
            /// https://thehackerdiary.wordpress.com/2017/04/21/exploring-devinput-1/
            /// </summary>
            public static async Task Watch(string inputEvent, Action<MouseInputEventData> callback, CancellationToken token)
            {
                TaskCompletionSource<bool> cancellationCompletionSource = new TaskCompletionSource<bool>();
                using (FileStream stream = new FileStream(Path.Combine(KeyboardDevPath, inputEvent), FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, true))
                using (token.Register(() => cancellationCompletionSource.TrySetResult(true)))
                {
                    while (!token.IsCancellationRequested)
                    {
                        int read = 0;
                        byte[] keyBuffer = new byte[MouseInputEventData.Size];
                        while (read < MouseInputEventData.Size && !token.IsCancellationRequested)
                        {
                            Task<int> readTask = stream.ReadAsync(keyBuffer, 0, MouseInputEventData.Size - read, token);
                            if (readTask != await Task.WhenAny(readTask, cancellationCompletionSource.Task))
                                throw new OperationCanceledException(token);
                            if (readTask.Exception?.InnerException != null)
                                throw readTask.Exception.InnerException;
                            read += readTask.Result;
                        }

                        // 40, 24, 56
                        MouseInputEventData eventData = new MouseInputEventData
                        {
                            Button = (MouseButton)(keyBuffer[0] == 40 || keyBuffer[0] == 24 || keyBuffer[0] == 56 ? 8 : keyBuffer[0]),
                            Position = new Vector2((sbyte)keyBuffer[1], (sbyte)-keyBuffer[2])
                        };
                        callback(eventData);
                    }
                }
            }
        }
    }

}