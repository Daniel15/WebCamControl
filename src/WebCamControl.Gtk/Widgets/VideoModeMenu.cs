// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using Gio;
using Gtk;
using WebCamControl.Core;
using WebCamControl.Gtk.Extensions;
using static WebCamControl.Core.Gettext;

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// Creates menus allowing the resolution, frame rate, and pixel format of the
/// camera to be changed.
/// </summary>
public class VideoModeMenu : VideoModeBase
{
	private const string _resolutionAction = "resolution";
	private const string _fpsAction = "fps";
	private const string _pixelFormatAction = "pixelFormat";
	private readonly RadioButtonSubmenu<ResolutionItem> _resolution;
	private readonly RadioButtonSubmenu<FrameRateItem> _frameRate;
	private readonly RadioButtonSubmenu<PixelFormatItem> _pixelFormat;
	
	public VideoModeMenu(
		ICameraManager cameraManager,
		ApplicationWindow window
	) : this(
		cameraManager,
		resolution: new RadioButtonSubmenu<ResolutionItem>(_resolutionAction, window),
		frameRate: new RadioButtonSubmenu<FrameRateItem>(_fpsAction, window),
		pixelFormat: new RadioButtonSubmenu<PixelFormatItem>(_pixelFormatAction, window)
	)
	{
	}

	private VideoModeMenu(
		ICameraManager cameraManager,
		RadioButtonSubmenu<ResolutionItem> resolution,
		RadioButtonSubmenu<FrameRateItem> frameRate,
		RadioButtonSubmenu<PixelFormatItem> pixelFormat
	) : base(
		cameraManager,
		resolution,
		frameRate,
		pixelFormat
	)
	{
		_resolution = resolution;
		_frameRate = frameRate;
		_pixelFormat = pixelFormat;
	}

	public void AppendToMenu(Menu menu)
	{
		menu.AppendSubmenu(_("Resolution"), _resolution);
		menu.AppendSubmenu(_("Frame Rate"), _frameRate);
		menu.AppendSubmenu(_("Pixel Format"), _pixelFormat);
	}
}
