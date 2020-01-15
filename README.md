# SharpPi
SharpPi is a high-level library that aims to bring more C# to the Raspberry Pi coding community. 
A high-level C# wrapper/utility library for Linux on the Raspberry Pi.
The low-level API will be exposed soon, once it's cleaned up.

# Features
- Draw graphics directly to the display (no X11 required) with OpenGL ([OpenTK](https://github.com/opentk/opentk))
- Integrates [Dear ImGUI](https://github.com/ocornut/imgui) with OpenGL in a simple to use API, using [ImGui.NET](https://github.com/mellinoe/ImGui.NET) as the underlying wrapper.
- Built-in web browser using ImGUI, including [HTML](https://html-agility-pack.net/), [JavaScript](https://github.com/sebastienros/esprima-dotnet), and [CSS](https://github.com/TylerBrinks/ExCSS) parsers.
- Encode/Decode/Manipulate video streams using [MMALSharp](https://github.com/techyian/MMALSharp/).
- Interface with GPIO pins, Bluetooth/Serial control, and more!
- Get raw input directly from Linux kernel (/dev/input), mouse and keyboard support built-in.
- Connect to wifi networks, create access points, scan for nearby access points, and more!
- Tcp client/server abstraction layer with [many options](https://github.com/tottaka/SharpPi).

# Installation
This will automatically install [Mono](https://www.mono-project.com/) and compile all [dependencies](https://github.com/tottaka/SharpPi/tree/master/deps/) needed for SharpPi to run.
Note: The setup script was meant to be run on the Raspberry Pi running [Raspbain OS](https://www.raspberrypi.org/downloads/raspbian/).
```
git clone https://github.com/tottaka/SharpPi # Download source code from git
cd SharpPi                                   # Enter the source directory
sudo bash setup.sh                           # Run the setup script, certain actions require sudo.
```

# References
- OpenTK: https://github.com/opentk/opentk
- Dear ImGUI: https://github.com/ocornut/imgui
- ImGui.NET: https://github.com/mellinoe/ImGui.NET
- MMALSharp: https://github.com/techyian/MMALSharp
- Esprima.NET: https://github.com/sebastienros/esprima-dotnet
- ExCSS: https://github.com/TylerBrinks/ExCSS
- Html Agility Pack: https://github.com/zzzprojects/html-agility-pack