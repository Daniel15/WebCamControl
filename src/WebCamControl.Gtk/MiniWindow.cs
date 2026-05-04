// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

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
	[Connect] private Menu _cameraMenu;
	
	private SimpleAction? _cameraAction;

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
		_cameraAction?.SetState(GLib.Variant.NewString(camera.RawName));
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
		_cameraAction = SimpleAction.NewStateful(
			"camera",
			GLib.VariantType.New("s"),
			GLib.Variant.NewString(_cameraManager.SelectedCamera.RawName)
		);
		_cameraAction.OnActivate += OnChangeCamera;
		AddAction(_cameraAction);
		
		// Remove placeholders from Blueprint
		_cameraMenu.RemoveAll();
		
		foreach (var camera in _cameraManager.Cameras)
		{
			var item = MenuItem.New($"{camera.Name} ({camera.RawName})", null);
			item.SetActionAndTargetValue(
				"win.camera", 
				GLib.Variant.NewString(camera.RawName)
			);
			_cameraMenu.AppendItem(item);
		}
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
	
	private void OnChangeCamera(SimpleAction sender, SimpleAction.ActivateSignalArgs args)
	{
		var selectedRawName = args.Parameter?.GetString(out var _);
		var newCamera = _cameraManager.Cameras.First(x => x.RawName == selectedRawName);
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
		
		if (_cameraAction is not null)
		{
			_cameraAction.OnActivate -= OnChangeCamera;
			RemoveAction(_cameraAction.Name!);
			_cameraAction.Dispose();
			_cameraAction = null;
		}
		_presets.OnChange -= InitializePresets;
		
		base.Dispose();
	}
}
