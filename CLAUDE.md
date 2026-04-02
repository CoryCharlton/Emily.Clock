# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commit Guidelines

- Do not include `Co-Authored-By` or any AI attribution in commit messages.
- When staging for a commit, use `git add -A` but flag any changes that appear
  unrelated to the current task and ask whether to include them.

## Project Overview

Emily.Clock is a .NET nanoFramework embedded application for a networked nightlight clock running on a LILYGO TTGO T4 V1.3 ESP32 device. It features a 2.4-inch 320x240 display, WS2812B RGB LED strip, and WiFi connectivity.

## Build & Test Commands

The solution is at `device\Emily.Clock.sln`. This is a .NET nanoFramework project using `.nfproj` files — use Visual Studio for building and deploying to hardware.

Run unit tests using the nanoFramework test runner with the settings file:
```
device\Emily.Clock.UnitTests\nano.runsettings
```

CI builds via GitHub Actions using `CCSWE-nanoframework/actions-nanoframework` reusable workflows (see `.github/workflows/build-solution.yml`).

## Architecture

The solution has three projects under `device/`:

- **Emily.Clock** — Hardware-agnostic core library. All business logic lives here.
- **Emily.Clock.App** — Device-specific implementations (GPIO pins, display driver, LED chipset). Entry point: `Program.cs` calls `DeviceHost.CreateDefaultBuilder().ConfigureHardware().AddCore()`.
- **Emily.Clock.UnitTests** — Tests for the core library.

### Key Patterns

**Dependency Injection / Hosting**: Uses `CCSWE.nanoFramework.Hosting`. Service registration is in `Emily.Clock/Bootstrapper.cs` (`AddCore` extension method). Hardware-specific registrations are in `Emily.Clock.App`.

**Initialization order**: Three `IDeviceInitializer` singletons execute sequentially: `DeviceInitialization` → `NetworkInitialization` → `ApplicationInitialization`.

**Mediator (pub/sub)**: `CCSWE.nanoFramework.Mediator` handles events (`ButtonEvent`, `TimeChangedEvent`, `DateChangedEvent`, `StatusEvent`). Subscribers are registered in `Bootstrapper.AddMediator()`.

**Configuration**: Typed configuration classes bound via `AddConfigurationManager().BindConfiguration(...)`. Configs: `DateTimeConfiguration`, `NightLightConfiguration`, `WirelessClientConfiguration`, `WirelessAccessPointConfiguration`.

**REST API**: HTTP server on port 80 via `CCSWE.nanoFramework.WebServer`. Controllers are in `Emily.Clock/Controllers/` (`DeviceController`, `ConfigurationController`).

**UI / Navigation**: `INavigationService` manages window transitions. Windows (`ClockWindow`, `ConfigurationWindow`, `NetworkFailureWindow`, `ResetToDefaultsWindow`) are registered as transient services and resolved via `IWindowFactory`.

**Hardware abstractions** (defined in `Emily.Clock`, implemented in `Emily.Clock.App`):
- `IButtonManager`, `IDisplayManager`, `ILedManager`, `IDeviceManager`, `IGpioProvider`

### Hardware Specifics (Emily.Clock.App)
- Display: SPI (MISO=12, MOSI=23, CLK=18, CS=27, DC=32, RST=5, BL=4)
- LED strip: WS2812B, 47 LEDs on pin 19; index 0 = Moon LED, index 1 = Sun LED

## Adding New Files

`.nfproj` files use old-style MSBuild format and do **not** auto-include source files. Every new `.cs` file must be manually added as a `<Compile Include="..." />` entry in the corresponding `.nfproj`.

## Unit Tests

Uses `nanoFramework.TestFramework`. Follow these conventions:
- Use `Assert.AreEqual` not `Assert.Equal` (the latter is obsolete)
- Do not add "Arrange / Act / Assert" comments in tests
- Mock implementations are in `Emily.Clock.UnitTests/Mocks/`
