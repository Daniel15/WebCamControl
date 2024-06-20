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
		_name.OnNotify += (_, args) =>
		{
			// TODO: This is messy. I couldn't find the editable signal in Gir.Core though
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

	private void Validate()
	{
		var isValid = _name.TextLength > 0;
		SetResponseEnabled("save", isValid);
	}

	private void Save()
	{
		_presets.SaveCurrent(_camera, _name.Text_);
	}
}
