// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using Adw;
using WebCamControl.Core;

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// Wraps <see cref="ComboRow"/>s for video modes.
/// </summary>
public class VideoModeComboRows : VideoModeBase
{
	public VideoModeComboRows(
		ICameraManager cameraManager,
		ComboRow resolution,
		ComboRow frameRate,
		ComboRow pixelFormat
	) : base(
		cameraManager,
		resolution: new CustomComboRow<ResolutionItem>(resolution),
		frameRate: new CustomComboRow<FrameRateItem>(frameRate),
		pixelFormat: new CustomComboRow<PixelFormatItem>(pixelFormat))
	{
	}
}
