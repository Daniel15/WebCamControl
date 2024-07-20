using System.Text.RegularExpressions;
using Adw;
using Gtk;
using WebCamControl.Core;

namespace WebCamControl.Gtk;

public class SavePresetDialog : Adw.AlertDialog
{
	private readonly ICamera _camera;
	private readonly IPresets _presets;
	
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
	[Connect] private readonly EntryRow _name = default!;
	[Connect] private readonly CustomComboRow<DestinationRow> _destination = default!;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
	
	public SavePresetDialog(ICamera camera, IPresets presets)
		: this(new Builder("SavePresetDialog.ui"), camera, presets)
	{
	}

	private SavePresetDialog(Builder builder, ICamera camera, IPresets presets)
		: base(builder.GetPointer("save_preset_dialog"), false)
	{
		_camera = camera;
		_presets = presets;
		builder.Connect(this);
		Validate();
		PopulateSaveDropdown();
		AttachEvents();
	}

	private void AttachEvents()
	{
		_name.OnNotify += (_, args) =>
		{
			// FIXME: This is messy. I couldn't find the editable signal in Gir.Core though
			// https://docs.gtk.org/gtk4/signal.Editable.changed.html
			if (args.Pspec.GetName() == "text")
			{
				Validate();
			}
		};
		OnResponse += (_, args) =>
		{
			if (args.Response == "save")
			{
				Save();
			}
		};
	}

	private void PopulateSaveDropdown()
	{
		var existingPresetOptions = _presets.PresetConfigs.Select(
			(config, index) => new DestinationRow(index, $"Replace #{index + 1}: {config.Name}")
		);
		_destination.LabelCallback = item => item.Name;
		_destination.Items = new[] { new DestinationRow(null, "New preset") }
			.Concat(existingPresetOptions);
	}

	private void Validate()
	{
		var isValid = _name.TextLength > 0;
		SetResponseEnabled("save", isValid);
	}

	private void Save()
	{
		_presets.SaveCurrent(
			_camera, 
			_name.Text_, 
			index: _destination.SelectedItem?.Index
		);
	}

	private record DestinationRow(
		int? Index,
		string Name
	);
}
