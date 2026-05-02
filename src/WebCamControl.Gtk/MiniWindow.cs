// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

using System.Globalization;
using Gtk;
using Microsoft.Extensions.DependencyInjection;
using WebCamControl.Core;
using WebCamControl.Gtk.Extensions;
using WebCamControl.Gtk.Widgets;
using static WebCamControl.Core.Gettext;

namespace WebCamControl.Gtk;

/// <summary>
/// Main window for the app - basic view
/// </summary>
[GObject.Subclass<Adw.ApplicationWindow>(qualifiedName: nameof(MiniWindow))]
[Template<EntryAssemblyResource>("MiniWindow.ui")]
public partial class MiniWindow : IWidgetWithServiceLocator<MiniWindow>
{
	private const int _minPresetButtonCount = 6;
	
	private ICamera _camera = null!;
	private IPresets _presets = null!;
	private EventHandler? _presetsChangedHandler;
	private EventHandler? _zoomChangedHandler;

	[Connect] private Box _panAndTiltButtons;
	[Connect] private Box _buttonsBox;
	[Connect] private Scale _zoom;

	public static MiniWindow New(IServiceProvider provider)
	{
		var app = provider.GetRequiredService<Adw.Application>();
		var cameraManager = provider.GetRequiredService<ICameraManager>();
		var presets = provider.GetRequiredService<IPresets>();
		var window = NewWithProperties([]);
		window.Configure(app, cameraManager, presets);
		return window;
	}

	private void Configure(Adw.Application app, ICameraManager cameraManager, IPresets presets)
	{
		_camera = cameraManager.SelectedCamera;
		_presets = presets;
		Application = app;
		Title = $"WebCamControl: {_camera.Name}";
		// TODO: Configure proper icon

		_panAndTiltButtons.Append(PanAndTiltButtons.New(_camera));
		InitializePresets();
		InitializeZoom();

		_presetsChangedHandler = (_, _) => InitializePresets();
		presets.OnChange += _presetsChangedHandler;
	}

	private void InitializePresets()
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
			button.OnClicked += (_, _) => _presets.ApplyTo(thisPreset, _camera);
		}
		else
		{
			button.TooltipText = _("No saved preset. Use the 'Save Preset' menu option or press Ctrl+S to save one.");			
		}
		return button;
	}

	private void InitializeZoom()
	{
		_zoom.DisableCameraControlIfUnsupported(_camera.Zoom);
		if (_camera.Zoom != null)
		{
			_zoom.OnChangeValue += (x, y) =>
			{
				_camera.Zoom.Value = (int)y.Value;
				return true;
			};
			_zoomChangedHandler = (_, _) => UpdateZoomState();
			_camera.Zoom.Changed += _zoomChangedHandler;
			_zoom.Adjustment = Adjustment.New(
				value: _camera.Zoom.Value,
				lower: _camera.Zoom.Minimum,
				upper: _camera.Zoom.Maximum,
				stepIncrement: _camera.Zoom.Step,
				pageIncrement: _camera.Zoom.Step,
				pageSize: 0
			);
			UpdateZoomState();
		}
	}

	private void UpdateZoomState()
	{
		if (_camera.Zoom == null)
		{
			return;
		}
		_zoom.SetValue(_camera.Zoom.Value);
		_zoom.Sensitive = _camera.Zoom.IsEnabled;
		_zoom.TooltipText = _($"Zoom: {_camera.Zoom.Value}%");
	}

	public override void Dispose()
	{
		if (_presetsChangedHandler != null)
		{
			_presets.OnChange -= _presetsChangedHandler;
			_presetsChangedHandler = null;
		}

		if (_zoomChangedHandler != null && _camera.Zoom != null)
		{
			_camera.Zoom.Changed -= _zoomChangedHandler;
			_zoomChangedHandler = null;
		}

		base.Dispose();
	}
}
