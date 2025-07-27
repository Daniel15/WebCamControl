// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

namespace WebCamControl.Core;

/// <summary>
/// A video mode supported by a camera
/// </summary>
public class VideoMode
{
	/// <summary>
	/// Width of the video (e.g. 3840 for 4K)
	/// </summary>
	public required uint Width { get; init; }
	
	/// <summary>
	/// Height of the video (e.g. 2160 for 4K)
	/// </summary>
	public required uint Height { get; init; }
	
	/// <summary>
	/// Frame rate (e.g. 30fps)
	/// </summary>
	public required uint FrameRate { get; init; }
	
	/// <summary>
	/// Name of the pixel format.
	/// </summary>
	public required string PixelFormatName { get; init; }
	
	/// <summary>
	/// Internal identifier for the pixel format.
	/// </summary>
	public required uint PixelFormatId { get; init; }
}
