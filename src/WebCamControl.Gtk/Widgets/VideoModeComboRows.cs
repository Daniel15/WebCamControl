// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using Adw;
using WebCamControl.Core;

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// Wraps <see cref="ComboRow"/>s for video modes.
/// </summary>
public class VideoModeComboRows : IDisposable
{
	private readonly ICameraManager _cameraManager;
	private readonly CustomComboRow<ResolutionItem> _resolution;
	private readonly CustomComboRow<uint> _frameRate;
	private readonly CustomComboRow<PixelFormatItem> _pixelFormat;
	
	public VideoModeComboRows(
		ICameraManager cameraManager,
		ComboRow resolution,
		ComboRow frameRate,
		ComboRow pixelFormat
	)
	{
		_cameraManager = cameraManager;
		_resolution = new CustomComboRow<ResolutionItem>(resolution);
		_frameRate = new CustomComboRow<uint>(frameRate);
		_pixelFormat = new CustomComboRow<PixelFormatItem>(pixelFormat);
		AttachEventListeners();
	}
	
	/// <summary>
	/// Build the lists of video modes.
	/// </summary>
	public void Initialize()
	{
		// Populate lists with all available options
		var resolutions = new List<ResolutionItem>();
		var frameRates = new HashSet<uint>();
		var pixelFormats = new List<PixelFormatItem>();
		foreach (var videoMode in _cameraManager.SelectedCamera.VideoModes)
		{
			resolutions.Add(new ResolutionItem(videoMode.Width, videoMode.Height));
			frameRates.Add(videoMode.FrameRate);
			pixelFormats.Add(new PixelFormatItem(videoMode.PixelFormatId, videoMode.PixelFormatName));
		}
		
		_resolution.Items = resolutions.Distinct();
		_frameRate.Items = frameRates;
		_pixelFormat.Items = pixelFormats.Distinct();
		UpdateSelection();
	}

	private void AttachEventListeners()
	{
		_resolution.LabelCallback = resolution => $"{resolution.Width}x{resolution.Height}";
		_resolution.OnSelectionChanged += (_, _) => SetMode(mode =>
			mode.Width == _resolution.SelectedItem?.Width &&
			mode.Height == _resolution.SelectedItem?.Height
		);
		
		_frameRate.LabelCallback = frameRate => $"{frameRate} fps";
		_frameRate.OnSelectionChanged += (_, _) => SetMode(mode =>
			mode.FrameRate == _frameRate.SelectedItem
		);
		
		_pixelFormat.LabelCallback = pixelFormat => pixelFormat.Name;
		_pixelFormat.OnSelectionChanged += (_, _) =>
			SetMode(mode => mode.PixelFormatId == _pixelFormat.SelectedItem?.Id);
	}

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
			var wrappedEx = new Exception("Could not set video mode. Is another app using the camera?", ex);
			ErrorDialog.ShowError(wrappedEx, null, null);
			// Reset controls back to previous mode
			UpdateSelection(camera.VideoMode);
		}
	}

	/// <summary>
	/// Updates the dropdown lists to show the selected mode.
	/// </summary>
	private void UpdateSelection(VideoMode? mode = null)
	{
		mode ??= _cameraManager.SelectedCamera.VideoMode;
		
		// Set selected modes
		// This works because .Equals() on records looks for value equality, not reference equality.
		_resolution.SelectedItem = new ResolutionItem(mode.Width, mode.Height);
		_frameRate.SelectedItem = mode.FrameRate;
		_pixelFormat.SelectedItem = new PixelFormatItem(mode.PixelFormatId, mode.PixelFormatName);
	}

	/// <inheritdoc />
	public void Dispose()
	{
		_resolution.Dispose();
		_frameRate.Dispose();
		_pixelFormat.Dispose();
	}

	/// <summary>
	/// An item in the resolution dropdown list.
	/// </summary>
	private record ResolutionItem(uint Width, uint Height);
	
	/// <summary>
	/// An item in the pixel format dropdown list.
	/// </summary>
	private record PixelFormatItem(uint Id, string Name);
}
