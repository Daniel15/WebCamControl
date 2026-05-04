// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

namespace WebCamControl.Core;

/// <summary>
/// Utilities for dealing with video modes.
/// </summary>
public static class VideoModeUtils
{
	/// <summary>
	/// Find the best new video mode based on the current mode and a filter. For example, when
	/// changing the FPS, find the mode with this FPS that most closely matches the other settings
	/// (resolution and pixel format).
	/// </summary>
	public static VideoMode FindBest(ICamera camera, Func<VideoMode, bool> predicate)
	{
		var currentMode = camera.VideoMode;
		return camera.VideoModes
			.Where(predicate)
			.OrderByDescending(newMode =>
			{
				// Count the number of properties that match between `currentMode` and `newMode`.
				var matches = 0;
				if (newMode.FrameRate == currentMode.FrameRate)
				{
					matches++;
				}
				if (newMode.PixelFormatId == currentMode.PixelFormatId)
				{
					matches++;
				}
				if (newMode.Width * newMode.Height == currentMode.Width * currentMode.Height)
				{
					matches++;
				}
				return matches;
			})
			.First();
	}
}
