using System;
using System.Collections.Generic;

namespace SharpPi
{
    /// <summary>
	/// Class for using the GPIO on the Raspberry Pi.
    /// Requires http://www.airspayce.com/mikem/bcm2835/, source should be included.
	/// </summary>
	public sealed class GPIO : IDisposable
    {
        public static bool Initialized { get; private set; }

        /// <summary>
        /// Specifies the direction of the GPIO pin (in/out)
        /// </summary>
        public enum Direction { INPUT, OUTPUT };

        /// <summary>
        /// The state of the pin (on/off)
        /// </summary>
        public enum PinState { HIGH, LOW };

        /// <summary>
        /// The internal pull up-down resistor of the pin (up/down)
        /// </summary>
        public enum Pullup { OFF = 0, DOWN = 1, UP = 2 };

        /// <summary>
        /// Refer to http://elinux.org/Rpi_Low-level_peripherals for diagram.
        /// </summary>
        public static class Pin
        {
            public const uint GPIO_NONE = uint.MaxValue;

            public const uint GPIO_00 = 0;
            public const uint GPIO_01 = 1;
            public const uint GPIO_04 = 4;
            public const uint GPIO_07 = 7;
            public const uint GPIO_08 = 8;
            public const uint GPIO_09 = 9;
            public const uint GPIO_10 = 10;
            public const uint GPIO_11 = 11;
            public const uint GPIO_14 = 14;
            public const uint GPIO_15 = 15;
            public const uint GPIO_17 = 17;
            public const uint GPIO_18 = 18;
            public const uint GPIO_21 = 21;
            public const uint GPIO_22 = 22;
            public const uint GPIO_23 = 23;
            public const uint GPIO_24 = 24;
            public const uint GPIO_25 = 25;

            public const uint P1_03 = 0;
            public const uint P1_05 = 1;
            public const uint P1_07 = 4;
            public const uint P1_08 = 14;
            public const uint P1_10 = 15;
            public const uint P1_11 = 17;
            public const uint P1_12 = 18;
            public const uint P1_13 = 21;
            public const uint P1_15 = 22;
            public const uint P1_16 = 23;
            public const uint P1_18 = 24;
            public const uint P1_19 = 10;
            public const uint P1_21 = 9;
            public const uint P1_22 = 25;
            public const uint P1_23 = 11;
            public const uint P1_24 = 8;
            public const uint P1_26 = 7;
            public const uint LED = 16;

            /// <summary>
            /// Raspberry Pi Rev.2 Models
            /// </summary>
            public static class Rev2
            {
                public const uint GPIO_27 = 27;
                public const uint GPIO_28 = 28;
                public const uint GPIO_29 = 29;
                public const uint GPIO_30 = 30;
                public const uint GPIO_31 = 31;

                public const uint P1_03 = 2;
                public const uint P1_05 = 3;
                public const uint P1_07 = 4;
                public const uint P1_13 = 27;

                public const uint P5_03 = 28;
                public const uint P5_04 = 29;
                public const uint P5_05 = 30;
                public const uint P5_06 = 31;

                /// <summary>
                /// Raspberry Pi A+/B+ Models
                /// </summary>
                public static class Plus
                {
                    public const uint P1_29 = 5;
                    public const uint P1_31 = 6;
                    public const uint P1_32 = 12;
                    public const uint P1_33 = 13;
                    public const uint P1_35 = 19;
                    public const uint P1_36 = 16;
                    public const uint P1_37 = 26;
                    public const uint P1_38 = 20;
                    public const uint P1_40 = 21;
                }
            }

            public enum Mask : uint
            {

                GPIO_NONE = uint.MaxValue,

                //Revision 1

                GPIO_00 = 0,
                GPIO_01 = 1,
                GPIO_04 = 1 << 4,
                GPIO_07 = 1 << 7,
                GPIO_08 = 1 << 8,
                GPIO_09 = 1 << 9,
                GPIO_10 = 1 << 10,
                GPIO_11 = 1 << 11,
                GPIO_14 = 1 << 14,
                GPIO_15 = 1 << 15,
                GPIO_17 = 1 << 17,
                GPIO_18 = 1 << 18,
                GPIO_21 = 1 << 21,
                GPIO_22 = 1 << 22,
                GPIO_23 = 1 << 23,
                GPIO_24 = 1 << 24,
                GPIO_25 = 1 << 25,

                Pin_P1_03 = 1 << 0,
                Pin_P1_05 = 1 << 1,
                Pin_P1_07 = 1 << 4,
                Pin_P1_08 = 1 << 14,
                Pin_P1_10 = 1 << 15,
                Pin_P1_11 = 1 << 17,
                Pin_P1_12 = 1 << 18,
                Pin_P1_13 = 1 << 21,
                Pin_P1_15 = 1 << 22,
                Pin_P1_16 = 1 << 23,
                Pin_P1_18 = 1 << 24,
                Pin_P1_19 = 1 << 10,
                Pin_P1_21 = 1 << 9,
                Pin_P1_22 = 1 << 25,
                Pin_P1_23 = 1 << 11,
                Pin_P1_24 = 1 << 8,
                Pin_P1_26 = 1 << 7,
                LED = 1 << 16,

                //Revision 2

                V2_GPIO_00 = 1 << 0,
                V2_GPIO_02 = 1 << 2,
                V2_GPIO_03 = 1 << 3,
                V2_GPIO_01 = 1 << 1,
                V2_GPIO_04 = 1 << 4,
                V2_GPIO_07 = 1 << 7,
                V2_GPIO_08 = 1 << 8,
                V2_GPIO_09 = 1 << 9,
                V2_GPIO_10 = 1 << 10,
                V2_GPIO_11 = 1 << 11,
                V2_GPIO_14 = 1 << 14,
                V2_GPIO_15 = 1 << 15,
                V2_GPIO_17 = 1 << 17,
                V2_GPIO_18 = 1 << 18,
                V2_GPIO_21 = 1 << 21,
                V2_GPIO_22 = 1 << 22,
                V2_GPIO_23 = 1 << 23,
                V2_GPIO_24 = 1 << 24,
                V2_GPIO_25 = 1 << 25,
                V2_GPIO_27 = 1 << 27,

                //Revision 2, new plug P5
                V2_GPIO_28 = 1 << 28,
                V2_GPIO_29 = 1 << 29,
                V2_GPIO_30 = 1 << 30,
                V2_GPIO_31 = (uint)1 << 31,

                V2_Pin_P1_03 = 1 << 2,
                V2_Pin_P1_05 = 1 << 3,
                V2_Pin_P1_07 = 1 << 4,
                V2_Pin_P1_08 = 1 << 14,
                V2_Pin_P1_10 = 1 << 15,
                V2_Pin_P1_11 = 1 << 17,
                V2_Pin_P1_12 = 1 << 18,
                V2_Pin_P1_13 = 1 << 27,
                V2_Pin_P1_15 = 1 << 22,
                V2_Pin_P1_16 = 1 << 23,
                V2_Pin_P1_18 = 1 << 24,
                V2_Pin_P1_19 = 1 << 10,
                V2_Pin_P1_21 = 1 << 9,
                V2_Pin_P1_22 = 1 << 25,
                V2_Pin_P1_23 = 1 << 11,
                V2_Pin_P1_24 = 1 << 8,
                V2_Pin_P1_26 = 1 << 7,
                V2_LED = 1 << 16,

                //Revision 2, new plug P5
                V2_Pin_P5_03 = 1 << 28,
                V2_Pin_P5_04 = 1 << 29,
                V2_Pin_P5_05 = 1 << 30,
                V2_Pin_P5_06 = (uint)1 << 31,

            };
        }

        private static readonly Dictionary<uint, GPIO> PinCache = new Dictionary<uint, GPIO>();

        /// <summary>
        /// The currently assigned GPIO pin.
        /// </summary>
        public readonly uint PinNumber;

        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Direction of the GPIO pin. (input/output)
        /// </summary>
		public Direction PinDirection
        {
            get => s_Direction;
            set
            {
                if (!IsDisposed && s_Direction != value)
                {
                    NativeMethods.bcm2835_gpio_fsel(PinNumber, (s_Direction = value) == Direction.OUTPUT);
                    if (s_Direction == Direction.INPUT)
                        Pull = Pullup.OFF;
                }
            }
        }
        private Direction s_Direction;

        /// <summary>
        /// The value of the GPIO pin. (on/off)
        /// </summary>
        public PinState State
        {
            get => s_State;
            set
            {
                if (!IsDisposed && s_State != value)
                    NativeMethods.bcm2835_gpio_write(PinNumber, (s_State = value) == PinState.HIGH);
            }
        }
        private PinState s_State = PinState.LOW;


        /// <summary>
        /// Internal pull up-down resistor for this pin.
        /// </summary>
        public Pullup Pull
        {
            get => s_Pull;
            set
            {
                if (!IsDisposed && s_Pull != value)
                    NativeMethods.bcm2835_gpio_set_pud(PinNumber, (uint)(s_Pull = value));
            }
        }
        private Pullup s_Pull = Pullup.OFF;

        /// <summary>
        /// Gets the bit mask of this pin.
        /// </summary>
        public Pin.Mask PinMask => (Pin.Mask)(1 << (ushort)PinNumber);

        /// <summary>
        /// Access to the specified GPIO setup with the specified direction with the specified initial value
        /// </summary>
        /// <param name="pin">The GPIO pin number. See the <see cref="Pin"/> class for various pin number constants.</param>
        /// <param name="direction">The direction of the pin.</param>
        /// <param name="initialValue">Value of the pin once it is initialized.</param>
        public GPIO(uint pin, Direction direction, PinState initialState = PinState.LOW)
        {
            if (pin == Pin.GPIO_NONE)
                throw new ArgumentException("Invalid pin");

            if (!Initialized)
                if (!(Initialized = NativeMethods.bcm2835_init()))
                    throw new InvalidOperationException("Could not initialize GPIO.");

            lock (PinCache)
            {
                if (PinCache.ContainsKey(pin))
                    throw new Exception(string.Format("Pin {0} is already being used. You must dispose the previous pin instance before creating a new one on the same pin.", pin));

                PinCache[pin] = this;
                PinNumber = pin;

                try
                {
                    PinDirection = direction;
                    State = initialState;
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }
        }

        ~GPIO()
        {
            Dispose();
        }

        /// <summary>
        /// Dispose of this GPIO pin instance.
        /// </summary>
        public void Dispose()
        {
            if (!IsDisposed)
            {
                lock (PinCache)
                    PinCache.Remove(PinNumber);

                IsDisposed = true;
            }
        }

        /// <summary>
		/// Sets any of the first 32 GPIO output pins specified in the mask to HIGH.
		/// </summary>
		/// <param name="mask">Mask of pins to affect. Use eg: (GPIOPinMask.GPIO_00) | GPIOPinMask.GPIO_01)</param>
		public static void SetMulti(Pin.Mask mask)
        {
            NativeMethods.bcm2835_gpio_set_multi((uint)mask);
        }

        /// <summary>
        /// Sets any of the first 32 GPIO output pins specified in the mask to LOW.
        /// </summary>
        /// <param name="mask">Mask of pins to affect. Use eg: (GPIOPinMask.GPIO_00) | GPIOPinMask.GPIO_01)</param>
        public static void ClearMulti(Pin.Mask mask)
        {
            NativeMethods.bcm2835_gpio_clr_multi((uint)mask);
        }

        /// <summary>
        /// Sets any of the first 32 GPIO output pins specified in the mask to value.
        /// </summary>
        /// <param name="mask">Mask of pins to affect. Use eg: (GPIOPinMask.GPIO_00) | GPIOPinMask.GPIO_01)</param>
        public static void WriteMulti(Pin.Mask mask, PinState state)
        {
            NativeMethods.bcm2835_gpio_write_multi((uint)mask, state == PinState.HIGH);
        }
    }

    /// <summary>
    /// Class for using SPI on the Raspberry Pi.
    /// </summary>
    public sealed class SPI
    {
        private GPIO CLK = new GPIO(GPIO.Pin.P1_18, GPIO.Direction.OUTPUT);
        private GPIO MISO = new GPIO(GPIO.Pin.P1_23, GPIO.Direction.INPUT);
        private GPIO MOSI = new GPIO(GPIO.Pin.P1_24, GPIO.Direction.OUTPUT);
        private GPIO CS = new GPIO(GPIO.Pin.P1_22, GPIO.Direction.OUTPUT);

        public SPI()
        {

        }
    }
}
