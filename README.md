# WebCamControl

### **NOTE: This app is currently under construction, and not designed to be used yet.**

---

WebCamControl is a Linux GUI app that can be used to control properties of your webcam such as pan, tilt, zoom, etc. You can run it to adjust the camera at the same time as other apps are using it.

It's primarily designed to be an alternative to the Insta360 Link Controller software for controlling Insta360 Link cameras, however it should work with any webcam supported by Video4Linux (V4L2).

# Features
 - **Presets**: Save and restore camera position.
 - **Non-exclusive access**: Can be used at the same time as other apps that use the webcam, such as video conferencing apps.

# Installation

TBD

# Development

1. Install [.NET 8.0](https://learn.microsoft.com/en-us/dotnet/core/install/linux)
2. Clone the repo
3. Run it using `dotnet run`:
```shell
dotnet run --project ./src/WebCamControl.Gtk
```
or with your favourite editor (e.g. Rider or VS Code).

To build the release version:
```shell
dotnet publish -c Release
```
