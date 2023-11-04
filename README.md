# Emily.Clock

This is a fun little nightlight clock project I created for my daughter. 

## Project Overview

### Features

- Functions as a nightlight with multiple color options and brightness levels
- Retrieves current time from WiFi connection
- REST API for configuration
- etc...
 
### Structure

One of the goals of the application was to design it in such a way that someone else, with different hardware, would be able to quickly and easily reuse the majority of my code 

**Emily.Clock** 

- Implements main application logic with the goal of being disconnected from the underlying hardware

**Emily.Clock.App**

- Implements device specific functionality is implemented (GPIO pin configuration, hardware specific implementations (display, LED chipset, etc.)

## Hardware

The 3D models and source code were designed around the following hardware:

- [LILYGO® TTGO T4 V1.3 2.4 inch](https://amzn.to/47eONBK)
- [MAX98357A I2S Audio Amplifier](https://amzn.to/3FKNz5n)
- [DS3231SN Real Time Clock](https://amzn.to/3st1r17)
- [4 Ohm 3 Watt Speaker](https://amzn.to/3siM7UU)

*Some of these links may be affiliate links, so I may earn a small commission when you make a purchase through these links at no additional cost to you.*

You can swap out any of the hardware and adjust the Fusion 360 model or design your own case.

## TODO

The more I worked on this the more I wanted to do with it. Currently the following features on the roadmap (in no particular order)

- Alarm functionality
- Audio provider (piezzo buzzer, I2S)
- Provide a web interface for some configuration (initial WiFi connection details, nightlight color and brightness, alarm and time settings)
- Battery powered RTC for situations where the WiFi is temporarily unavailable

## Background and History

Initially I started the project using [PlatformIO](https://platformio.org/), which is a great toolchain/IDE, but quickly got burnt out with C/C++ so the project stalled. At the time I was aware of the Python solutions available for embedded development but I dislike working in Python even more than C/C++ so...

Luckily [José Simões](https://github.com/josesimoes) joined the [Unexpected Maker](https://unexpectedmaker.com/) Discord and mentioned [.NET nanoFramework](https://www.nanoframework.net/). Being that my day job is a software engineer working on the .NET Core stack it was a no brainer for me to checkout [.NET nanoFramework](https://www.nanoframework.net/).

I've been very impressed with [.NET nanoFramework](https://www.nanoframework.net/) and the surrounding community. I cannot recommend it enough for anyone the enjoys working with C# and wants to accelerate their embedded development.
