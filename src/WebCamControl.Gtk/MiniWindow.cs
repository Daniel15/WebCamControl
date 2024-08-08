using System.Globalization;
using Gtk;
using WebCamControl.Core;
using WebCamControl.Gtk.Extensions;
using WebCamControl.Gtk.Widgets;

namespace WebCamControl.Gtk;

/// <summary>
/// Main window for the app - basic view
/// </summary>
public class MiniWindow : Adw.Window
{
	private const float _panTiltAdjustmentAmount = 2f;
	private const int _minPresetButtonCount = 6;
	
	private readonly ICamera _camera;
	private readonly IPresets _presets;

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
	[Connect] private readonly PressAndHoldButton _up = default!;
	[Connect] private readonly PressAndHoldButton _down = default!;
	[Connect] private readonly PressAndHoldButton _left = default!;
	[Connect] private readonly PressAndHoldButton _right = default!;
	[Connect] private readonly Box _buttonsBox = default!;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

	public MiniWindow(
		Adw.Application app,
		ICamera camera,
		IPresets presets
	) : this(new Builder("MiniWindow.ui"), camera, presets)
	{
		Application = app;
	}

	private MiniWindow(
		Builder builder,
		ICamera camera,
		IPresets presets
	) : base(builder.GetPointer("mini_window"), false)
	{
		_camera = camera;
		_presets = presets;
		builder.Connect(this);
		Title = "WebCamControl: " + _camera.Name;
		// TODO: Configure proper icon
		
		InitializePanAndTilt();
		InitializePresets();

		presets.OnChange += (_, _) => InitializePresets();
	}

	private void InitializePanAndTilt()
	{
		_left.DisableCameraControlIfUnsupported(_camera.Pan);
		_right.DisableCameraControlIfUnsupported(_camera.Pan);
		if (_camera.Pan != null)
		{
			_left.OnHeld += (_, _) => _camera.Pan.Value -= _panTiltAdjustmentAmount;
			_right.OnHeld += (_, _) => _camera.Pan.Value += _panTiltAdjustmentAmount;
		}

		_down.DisableCameraControlIfUnsupported(_camera.Tilt);
		_up.DisableCameraControlIfUnsupported(_camera.Tilt);
		if (_camera.Tilt != null)
		{
			_down.OnHeld += (_, _) => _camera.Tilt.Value -= _panTiltAdjustmentAmount;
			_up.OnHeld += (_, _) => _camera.Tilt.Value += _panTiltAdjustmentAmount;
		}
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
