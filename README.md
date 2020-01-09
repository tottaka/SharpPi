# SharpPi
A high-level C# wrapper/utility library for Linux on the Raspberry Pi.
Access to the low-level API will be available soon, once it's cleaned up.

What it can do:
- Draw graphics directly to the display (no X11 required) with OpenGL ([OpenTK](https://github.com/opentk/opentk))
- Integrates [Dear ImGUI](https://github.com/ocornut/imgui) with OpenGL in a simple to use API, using [ImGui.NET](https://github.com/mellinoe/ImGui.NET) as the underlying wrapper.
- Get raw input directly from Linux kernel (/dev/input), mouse and keyboard support built-in.
- Encode/Decode H264 video streams using [Cisco's OpenH264](https://github.com/cisco/openh264) implementation.
- Interface with GPIO pins, Bluetooth/Serial control, and more!
- Connect to wifi networks, create access points, scan for nearby access points, and more!

Features:
- A wrapper for encoding/decoding video streams.
- A wrapper for drawing graphics with OpenGL.
- A GPIO utility library for interfacing with the Raspberry Pi GPIO.
- An OS utility library for doing things such as connecting to/scanning for wifi networks, setting device hostname, get rpi model type, start processes/get output, and much more!

References:
- OpenTK: https://github.com/opentk/opentk
- Dear ImGUI: https://github.com/ocornut/imgui
- ImGui.NET: https://github.com/mellinoe/ImGui.NET
- Cisco's OpenH264: https://github.com/cisco/openh264

Notes:
All of the included compiled libaries were built on the Raspberry Pi 3B+ in Raspbain Buster.