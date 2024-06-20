using System.Globalization;
using Gtk;
using Microsoft.Extensions.Options;
using WebCamControl.Core;
using WebCamControl.Core.Configuration;
using WebCamControl.Gtk.Extensions;
using WebCamControl.Gtk.Widgets;

namespace WebCamControl.Gtk;

/// <summary>
/// Main window for the app.
/// </summary>
public class MainWindow : Adw.Window
{
	private const float _panTiltAdjustmentAmount = 2f;
	private const int _minPresetButtonCount = 6;
	
	private readonly ICamera _camera;
	private readonly IPresets _presets;
	private readonly IOptionsMonitor<Config> _config;

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
	[Connect] private readonly PressAndHoldButton _up = default!;
	[Connect] private readonly PressAndHoldButton _down = default!;
	[Connect] private readonly PressAndHoldButton _left = default!;
	[Connect] private readonly PressAndHoldButton _right = default!;
	[Connect] private readonly Box _buttonsBox = default!;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

	public MainWindow(
		Adw.Application app,
		ICamera camera,
		IPresets presets
	) : this(new Builder("MainWindow.ui"), camera, presets)
	{
		Application = app;
	}

	private MainWindow(
		Builder builder,
		ICamera camera,
		IPresets presets
	) : base(builder.GetPointer("main_window"), false)
	{
		_camera = camera;
		_presets = presets;
		builder.Connect(this);
		Title = "WebCamControl: " + _camera.Name;
		
		InitializePanAndTilt();
		InitializePresets();

		presets.OnChange += (_, _) => InitializePresets();
	}

	private void InitializePanAndTilt()
	{
		DisableIfUnsupported(_left, _camera.Pan);
		DisableIfUnsupported(_right, _camera.Pan);
		if (_camera.Pan != null)
		{
			_left.OnHeld += (_, _) => _camera.Pan.Value -= _panTiltAdjustmentAmount;
			_right.OnHeld += (_, _) => _camera.Pan.Value += _panTiltAdjustmentAmount;
		}

		DisableIfUnsupported(_down, _camera.Tilt);
		DisableIfUnsupported(_up, _camera.Tilt);
		if (_camera.Tilt != null)
		{
			_down.OnHeld += (_, _) => _camera.Tilt.Value -= _panTiltAdjustmentAmount;
			_up.OnHeld += (_, _) => _camera.Tilt.Value += _panTiltAdjustmentAmount;
		}
	}

	private void DisableIfUnsupported(Button button, object? control)
	{
		var isSupported = control != null;
		button.Sensitive = isSupported;
		button.TooltipText = isSupported 
			? null 
			: "Not supported by this camera";
	}

	private void InitializePresets()
	{
		_buttonsBox.RemoveChildren();
		var presetCount = _presets.PresetConfigs.Count - 1;
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
			button.TooltipText = $"Apply preset \"{thisPreset.Name}\"";
			button.OnClicked += (_, _) => _presets.ApplyTo(thisPreset, _camera);
		}
		else
		{
			button.TooltipText = "No saved preset. Use the 'Save Preset' menu option or press Ctrl+S to save one.";			
		}
		return button;
	}
}
