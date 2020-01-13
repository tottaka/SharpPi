# SharpPi
SharpPi is a high-level library that aims to bring more C# to the Raspberry Pi coding community. 
A high-level C# wrapper/utility library for Linux on the Raspberry Pi.
The low-level API will be exposed soon, once it's cleaned up.

# Features
- Draw graphics directly to the display (no X11 required) with OpenGL ([OpenTK](https://github.com/opentk/opentk))
- Integrates [Dear ImGUI](https://github.com/ocornut/imgui) with OpenGL in a simple to use API, using [ImGui.NET](https://github.com/mellinoe/ImGui.NET) as the underlying wrapper.
- Get raw input directly from Linux kernel (/dev/input), mouse and keyboard support built-in.
- Encode/Decode/Manipulate video streams using [MMALSharp](https://github.com/techyian/MMALSharp/).
- Interface with GPIO pins, Bluetooth/Serial control, and more!
- Connect to wifi networks, create access points, scan for nearby access points, and more!
- Tcp client/server abstraction layer with [many options](https://github.com/tottaka/SharpPi).

# References:
- OpenTK: https://github.com/opentk/opentk
- Dear ImGUI: https://github.com/ocornut/imgui
- ImGui.NET: https://github.com/mellinoe/ImGui.NET
- MMALSharp: https://github.com/techyian/MMALSharp

# Installation
```
git clone https://github.com/tottaka/SharpPi # Download source code from git
cd SharpPi									 # Enter the source directory
./setup.sh									 # Run the setup script
```
This will automatically install & compile all dependencies and other resources needed for SharpPi to run.


# Note
All of the included compiled native libaries were built on the Raspberry Pi 3B+ in Raspbain Buster