# SharpPi
SharpPi is a high-level C# wrapper/utility library for Linux on the Raspberry Pi.
Access to the low-level API will be available soon, once it's cleaned up.

What it can do:
- Draw graphics directly to the display (no X11 required) with OpenGL.
- Integrates Dear ImGUI (https://github.com/ocornut/imgui) with OpenGL in a simple to use API, using ImGui.NET (https://github.com/mellinoe/ImGui.NET) as the underlaying wrapper.
- Get raw input directly from Linux kernel (/dev/input), mouse and keyboard support built-in.
- Encode/Decode H264 video streams using Cisco's OpenH264 implementation. (https://github.com/cisco/openh264)
- Interface with GPIO pins, Bluetooth/Serial control, and more!
- Connect to wifi networks, create access points, scan for nearby access points, and more!
- Start processes and get the output.

Features:
- A wrapper for encoding/decoding video streams with Cisco's OpenH264 implementation.
- An opengl wrapper (OpenTK) for drawing graphics directly to the screen (no X required.)
- A GPIO utility library for interfacing with the Raspberry Pi GPIO.
- An OS utility library for doing things such as connecting to/scanning for wifi networks, setting device hostname, get rpi model type, easily start subprocesses & process output, and much more!
