using System.Text.RegularExpressions;
using Adw;
using Gtk;
using WebCamControl.Core;

namespace WebCamControl.Gtk;

public partial class SavePresetDialog : Adw.AlertDialog
{
	[GeneratedRegex(@"^Replace #(?<index>\d+):")]
	private static partial Regex SaveAsNameRegex();
	
	private readonly ICamera _camera;
	private readonly IPresets _presets;
	
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
	[Connect] private readonly EntryRow _name = default!;
	[Connect] private readonly ComboRow _destination = default!;
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
		var options = new List<string>
		{
			"New preset"
		};
		
		var presetCount = _presets.PresetConfigs.Count;
		for (var i = 0; i < presetCount; i++)
		{
			var preset = _presets.PresetConfigs[i];
			options.Add($"Replace #{i + 1}: {preset.Name}");
		}

		var model = StringList.New(options.ToArray());
		_destination.Model = model;
	}

	private void Validate()
	{
		var isValid = _name.TextLength > 0;
		SetResponseEnabled("save", isValid);
	}

	private void Save()
	{
		var destination = (StringObject?)_destination.SelectedItem;
		int? index = null;

		if (destination != null && destination.String != null)
		{
			// FIXME: This is super sketchy, but Gir.Core doesn't properly support ListStores
			// at the moment: https://github.com/gircore/gir.core/discussions/1099
			var matches = SaveAsNameRegex().Matches(destination.String);
			if (matches.Count > 0)
			{
				index = int.Parse(matches[0].Groups["index"].Value);
			}
		}
		
		_presets.SaveCurrent(_camera, _name.Text_, index);
	}
}
