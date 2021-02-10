# SharpPi
Forget Python, we're bringing practical C# to the [Raspberry Pi](https://www.raspberrypi.org/) by combining many wrapper/utility libraries into a simple to use, high-level API. It was designed to run on [Raspbain Linux](https://www.raspberrypi.org/software/operating-systems/) (Typically the Lite version.) But is usable with any device that is running Linux with [Mono (6.12)](https://www.mono-project.com/).

SharpPi is a large project, and the low-level API will be exposed eventually - once it's cleaned up.

# Features
- Draw graphics directly to the display (no X11 required)
# What can I do with this?
Many things:
- [x] Create hardware-accelerated 2D GUI (and 3D games/simulations.)
- [x] Stream/Encode H.264 camera video over networks.
- [ ] Play/decode video with FFmpeg.
- [x] Play/decode video with hardware-accelerated H.264.
- [ ] Render web pages using the hardware-accelerated 2D rendering engine in OpenGL. (with WebGL support on the roadmap)
- [x] Connect to/create WiFi networks, manage network firewalls(via ip-tables), scan for nearby access points, and more!
- [x] Read/Write to input devices such as Mouse, Keyboard, Gamepads (such as Xbox, and PlayStation controllers), and custom/proprietary devices.
- [x] Asyncronously run programs and register/manage [systemd](https://systemd.io/) services.
- [x] Interface with GPIO & Bluetooth/Serial devices.
- [x] TCP & UDP socket client/server abstraction layer with [many options](https://github.com/tottaka/SharpPi).

# Installation
I will not include any prebuilt native libraries, so you will have to compile them on your own, however, I have included a simple bash script to automate the installation and compilation process.
Run the following commands to automatically install Mono and compile all [native dependencies](https://github.com/tottaka/SharpPi/tree/master/deps/) needed for SharpPi to run.
```
git clone https://github.com/tottaka/SharpPi # Download source code from git
cd SharpPi                                   # Enter the source directory
sudo bash setup.sh                           # Run the setup script, requires sudo for certain actions.
```
If you do not have git, install it with '```sudo apt install git```'

# Credits
- [OpenTK v3](https://github.com/opentk/opentk/tree/3.x)
- [Dear ImGUI](https://github.com/ocornut/imgui), [ImGui.NET](https://github.com/mellinoe/ImGui.NET)
- [MMALSharp](https://github.com/techyian/MMALSharp
- WebRenderer: [HAP](https://html-agility-pack.net/), [Jint](https://github.com/sebastienros/jint), and [ExCSS](https://github.com/TylerBrinks/ExCSS).
