// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using WebCamControl.Core;
using static WebCamControl.Core.Gettext;

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// Base class for widgets that deal with video modes.
/// </summary>
public abstract class VideoModeBase : IDisposable
{
	private readonly ICameraManager _cameraManager;
	private readonly IListWidget<ResolutionItem> _resolution;
	private readonly IListWidget<FrameRateItem> _frameRate;
	private readonly IListWidget<PixelFormatItem> _pixelFormat;

	protected VideoModeBase(
		ICameraManager cameraManager,
		IListWidget<ResolutionItem> resolution,
		IListWidget<FrameRateItem> frameRate,
		IListWidget<PixelFormatItem> pixelFormat
	)
	{
		_cameraManager = cameraManager;
		_resolution = resolution;
		_frameRate = frameRate;
		_pixelFormat = pixelFormat;

		AttachEventListeners();
	}
	
	/// <summary>
	/// Build the lists of video modes.
	/// </summary>
	public void Initialize()
	{
		// Populate lists with all available options
		var resolutions = new HashSet<ResolutionItem>();
		var frameRates = new HashSet<FrameRateItem>();
		var pixelFormats = new HashSet<PixelFormatItem>();
		foreach (var videoMode in _cameraManager.SelectedCamera.VideoModes)
		{
			resolutions.Add(new ResolutionItem(videoMode.Width, videoMode.Height));
			frameRates.Add(new FrameRateItem(videoMode.FrameRate));
			pixelFormats.Add(new PixelFormatItem(videoMode.PixelFormatId, videoMode.PixelFormatName));
		}
		
		_resolution.Items = resolutions;
		_frameRate.Items = frameRates;
		_pixelFormat.Items = pixelFormats;
		
		UpdateSelection(_cameraManager.SelectedCamera.VideoMode);
	}

	private void AttachEventListeners()
	{
		_resolution.SelectionChanged += (_, resolution) => SetMode(mode =>
			mode.Width == resolution?.Width &&
			mode.Height == resolution?.Height
		);
		_frameRate.SelectionChanged +=
			(_, frameRate) => SetMode(mode => mode.FrameRate == frameRate?.FrameRate);
		_pixelFormat.SelectionChanged +=
			(_, pixelFormat) => SetMode(mode => mode.PixelFormatId == pixelFormat?.Id);
	}
	
	/// <summary>
	/// Sets the video mode to the best one that matches the provided predicate.
	/// </summary>
	private void SetMode(Func<VideoMode, bool> predicate)
	{
		var camera = _cameraManager.SelectedCamera;
		var newMode = VideoModeUtils.FindBest(camera, predicate);
		
		if (newMode == camera.VideoMode)
		{
			// New mode is the same as the old mode.
			return;
		}

		try
		{
			camera.VideoMode = newMode;
			UpdateSelection(newMode);
		}
		catch (Exception ex)
		{
			var wrappedEx = new Exception(
				_("Could not set video mode. To change the video mode, you will need to " +
				  "stop any active streams."),
				ex
			);
			ErrorDialog.ShowError(wrappedEx, null, null);
			// Reset controls back to previous mode
			UpdateSelection(camera.VideoMode);
		}
	}

	/// <summary>
	/// Updates the UI to show the selected mode.
	/// </summary>
	private void UpdateSelection(VideoMode mode)
	{
		// Set selected modes
		// This works because .Equals() on records looks for value equality, not reference equality.
		_resolution.SelectedItem = new ResolutionItem(mode.Width, mode.Height);
		_frameRate.SelectedItem = new FrameRateItem(mode.FrameRate);
		_pixelFormat.SelectedItem = new PixelFormatItem(mode.PixelFormatId, mode.PixelFormatName);
	}

	public void Dispose()
	{
		if (_resolution is IDisposable resolution)
		{
			resolution.Dispose();
		}
		if (_frameRate is IDisposable frameRate)
		{
			frameRate.Dispose();
		}
		if (_pixelFormat is IDisposable pixelFormat)
		{
			pixelFormat.Dispose();
		}
	}
}

/// <summary>
/// An item in the resolution dropdown list.
/// </summary>
public record ResolutionItem(uint Width, uint Height)
{
	public override string ToString() => $"{Width} x {Height}";
}

/// <summary>
/// An item in the frame rate dropdown list
/// </summary>
public record FrameRateItem(uint FrameRate)
{
	public override string ToString() => $"{FrameRate} fps";
}

/// <summary>
/// An item in the pixel format dropdown list.
/// </summary>
public record PixelFormatItem(uint Id, string Label)
{
	public override string ToString() => Label;
};
