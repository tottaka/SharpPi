#!/bin/bash

# make sure we're up to date on everything
sudo apt-get update
sudo apt-get upgrade

# install mono first
sudo apt install apt-transport-https dirmngr gnupg ca-certificates
sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb https://download.mono-project.com/repo/debian stable-raspbianbuster main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
sudo apt update
sudo apt install mono-complete

cd deps

# install and compile GPIO library
echo "Installing bcm2835-1.60"
cd bcm2835-1.60
#sudo bash configure
#make
#sudo make check
#sudo make install
#sudo cp cimgui.so /usr/lib/libcimgui.so

# The Makefile for his library compiles a shared object where a statically linked library is required.
# To compile a statically linked binary, do the following:
# tar -zxf bcm2835-1.3.tar.gz
# cd bcm2835-1.3/src
# make libbcm2835.a
# cc -shared bcm2835.o -o libbcm2835.so
cd ../

# install and compile cimgui
echo "Installing cimgui"
cd cimgui

make
echo "Copying 'cimgui.so' to /usr/lib"
sudo cp cimgui.so /usr/lib/libcimgui.so
cd ../

# finish and clean up
cd ../
echo "Done."