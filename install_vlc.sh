#!/bin/bash

# Installation script for VLC libraries required by LibVLCSharp
# This script needs to be run with sudo privileges

echo "================================"
echo "VLC Installation for Media Player"
echo "================================"
echo ""
echo "This will install VLC libraries required for in-app video and music playback."
echo ""

# Check if running as root
if [ "$EUID" -ne 0 ]; then 
    echo "Please run with sudo: sudo bash install_vlc.sh"
    exit 1
fi

echo "Updating package list..."
apt-get update

echo ""
echo "Installing VLC and required libraries..."
apt-get install -y vlc libvlc-dev libvlccore-dev

echo ""
echo "Checking installation..."
vlc --version | head -1

echo ""
echo "================================"
echo "Installation complete!"
echo "================================"
echo ""
echo "You can now run your application with in-app video and music playback."
echo "To run the application, use: dotnet run --project avaloniaubuntuqoder20260128/avaloniaubuntuqoder20260128.csproj"
