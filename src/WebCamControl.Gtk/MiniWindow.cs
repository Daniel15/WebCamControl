// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>
// https://d.sb/wcc

using System.Globalization;
using Gio;
using Gtk;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebCamControl.Core;
using WebCamControl.Gtk.Extensions;
using WebCamControl.Gtk.Widgets;
using static WebCamControl.Core.Gettext;
using Range = Gtk.Range;

namespace WebCamControl.Gtk;

/// <summary>
/// Main window for the app - basic view
/// </summary>
[GObject.Subclass<Adw.ApplicationWindow>(qualifiedName: nameof(MiniWindow))]
[Template<EntryAssemblyResource>("MiniWindow.ui")]
public partial class MiniWindow : IWidgetWithServiceLocator<MiniWindow>
{
	private const int _minPresetButtonCount = 6;
	
	private ICameraManager _cameraManager = null!;
	private ILogger<MiniWindow> _logger = null!;
	private IPresets _presets = null!;
	private IServiceProvider _provider = null!;

	[Connect] private Box _panAndTiltButtons;
	[Connect] private Box _buttonsBox;
	[Connect] private Scale _zoom;
	[Connect] private Menu _cameraMenuSection;

	private RadioButtonSubmenu<CameraWrapper>? _cameraMenu;

	public static MiniWindow New(IServiceProvider provider)
	{
		var app = provider.GetRequiredService<Adw.Application>();
		var cameraManager = provider.GetRequiredService<ICameraManager>();
		var presets = provider.GetRequiredService<IPresets>();
		var logger = provider.GetRequiredService<ILogger<MiniWindow>>();
		var window = NewWithProperties([]);
		window.Configure(app, provider, cameraManager, presets, logger);
		return window;
	}

	private void Configure(
		Adw.Application app,
		IServiceProvider provider,
		ICameraManager cameraManager,
		IPresets presets,
		ILogger<MiniWindow> logger
	)
	{
		_cameraManager = cameraManager;
		_provider = provider;
		_presets = presets;
		_logger = logger;
		Application = app;
		// TODO: Configure proper icon
		
		InitializePresets();
		InitializeMenus();
		InitializeCamera();
		
		presets.OnChange += InitializePresets;
	}

	private void InitializeCamera()
	{
		var camera = _cameraManager.SelectedCamera;
		_logger.LogInformation("Initializing controls for {CameraName}", camera.Name);
		Title = $"WebCamControl: {camera.Name}";
		
		_panAndTiltButtons.Append(PanAndTiltButtons.New(camera));
		
		InitializeZoom();
		CheckOutOfRangeControls();
		_cameraMenu?.SelectedItem = new CameraWrapper(camera);
	}

	private void InitializePresets(object? sender = null, EventArgs? args = null)
	{
		_buttonsBox.RemoveChildren();
		var presetCount = _presets.PresetConfigs.Count;
		var buttonCount = Math.Max(presetCount, _minPresetButtonCount);
		Button? prevButton = null;
		for (var i = 0; i < buttonCount; i++)
		{
			var button = InitializePresetButton(i);
			_buttonsBox.InsertChildAfter(button, prevButton);
			prevButton = button;
		}
	}

	private Button InitializePresetButton(int index)
	{
		var isEnabled = index < _presets.PresetConfigs.Count;
		var button = Button.New();
		button.Label = (index + 1).ToString(CultureInfo.InvariantCulture);
		button.Sensitive = isEnabled;
		if (isEnabled)
		{
			var thisPreset = _presets.PresetConfigs[index];
			button.TooltipText = _($"Apply preset \"{thisPreset.Name}\"");
			button.OnClicked += (_, _) =>
				_presets.ApplyTo(thisPreset, _cameraManager.SelectedCamera);
		}
		else
		{
			button.TooltipText = _("No saved preset. Use the 'Save Preset' menu option or press Ctrl+S to save one.");			
		}
		return button;
	}

	private void InitializeZoom()
	{
		var camera = _cameraManager.SelectedCamera;
		_zoom.DisableCameraControlIfUnsupported(camera.Zoom);
		if (camera.Zoom != null)
		{
			_zoom.OnChangeValue += OnChangeZoom;
			camera.Zoom.Changed += UpdateZoomState;
			_zoom.Adjustment = Adjustment.New(
				value: camera.Zoom.Value,
				lower: camera.Zoom.Minimum,
				upper: camera.Zoom.Maximum,
				stepIncrement: camera.Zoom.Step,
				pageIncrement: camera.Zoom.Step,
				pageSize: 0
			);
			UpdateZoomState();
		}
	}

	private void InitializeMenus()
	{
		var cameraMenu = new RadioButtonSubmenu<CameraWrapper>("camera", this)
		{
			Items = _cameraManager.Cameras.Select(x => new CameraWrapper(x)),
			SelectedItem = new CameraWrapper(_cameraManager.SelectedCamera),
		};
		cameraMenu.SelectionChanged += OnChangeCamera;
		_cameraMenuSection.AppendSubmenu(_("Camera"), cameraMenu);
		_cameraMenu = cameraMenu;
	}

	private void UpdateZoomState(object? sender = null, EventArgs? args = null)
	{
		var camera = _cameraManager.SelectedCamera;
		if (camera.Zoom == null)
		{
			return;
		}
		_zoom.SetValue(camera.Zoom.Value);
		_zoom.Sensitive = camera.Zoom.IsEnabled;
		_zoom.TooltipText = _($"Zoom: {camera.Zoom.Value}%");
	}
	
	private void CheckOutOfRangeControls()
	{
		var detector = ActivatorUtilities.CreateInstance<OutOfRangeDetector>(
			_provider,
			_cameraManager.SelectedCamera
		);
		var outOfRange = detector.Detect().ToArray();
		if (outOfRange.Length != 0)
		{
			OutOfRangeDialog.Show(outOfRange, this);
		}
	}
	
	private void OnChangeCamera(object? sender, CameraWrapper? newCameraWrapper)
	{
		var newCamera = newCameraWrapper?.Camera;
		if (newCamera is null)
		{
			return;
		}
		CleanupCamera();
		_logger.LogInformation("Changing camera to {CameraName}", newCamera.Name);
		_cameraManager.SelectedCamera = newCamera;
		InitializeCamera();
	}
	
	private bool OnChangeZoom(Range _, Range.ChangeValueSignalArgs range)
	{
		var camera = _cameraManager.SelectedCamera;
		camera.Zoom?.Value = (int)range.Value;
		return true;
	}
	
	/// <summary>
	/// Removes event handlers, controls, etc. for the current camera. Essentially,
	/// undoes everything that <see cref="InitializeCamera"/> did.
	/// </summary>
	private void CleanupCamera()
	{
		var camera = _cameraManager.SelectedCamera;
		_panAndTiltButtons.RemoveChildren();
		_zoom.OnChangeValue -= OnChangeZoom;
		camera.Zoom?.Changed -= UpdateZoomState;
	}

	public override void Dispose()
	{
		CleanupCamera();

		if (_cameraMenu is not null)
		{
			_cameraMenu.Dispose();
			_cameraMenu = null;
		}
		_presets.OnChange -= InitializePresets;
		
		base.Dispose();
	}

	/// <summary>
	/// Wrapper around <see cref="ICamera"/> that adds the <see cref="IListItem"/> interface.
	/// </summary>
	private record CameraWrapper(ICamera Camera) : IListItem
	{
		public string Label => $"{Camera.Name} ({Camera.RawName})";
	}
}
