using Adw;
using GObject;
using Gtk;
using Microsoft.Extensions.DependencyInjection;
using WebCamControl.Core;
using WebCamControl.Gtk;
using static WebCamControl.Core.Gettext;

namespace WebCamControl.GtkViews;

[GObject.Subclass<Adw.AlertDialog>(qualifiedName: nameof(SavePresetDialog))]
[global::Gtk.Template<global::Gtk.AssemblyResource>("SavePresetDialog.ui")]
public partial class SavePresetDialog
{
	private const string _changedSignalName = "changed";
	private static readonly Signal<EntryRow> _changedSignal =
		new(_changedSignalName, _changedSignalName);

	private ICamera _camera = null!;
	private IPresets _presets = null!;
	private CustomComboRow<DestinationRow> _destinationCombo = null!;
	
	[global::Gtk.Connect] private EntryRow _name;
	[global::Gtk.Connect] private ComboRow _destination;
	
	public static SavePresetDialog Create(IServiceProvider provider)
	{
		var cameraManager = provider.GetRequiredService<ICameraManager>();
		var presets = provider.GetRequiredService<IPresets>();
		var dialog = NewWithProperties([]);
		dialog.Configure(cameraManager, presets);
		return dialog;
	}

	private void Configure(ICameraManager cameraManager, IPresets presets)
	{
		_camera = cameraManager.SelectedCamera;
		_presets = presets;
		Validate();
		PopulateSaveDropdown();
		AttachEvents();
	}

	private void AttachEvents()
	{
		_name.OnEntryActivated += (_, _) => Save();
		_changedSignal.Connect(_name, (_, _) => Validate());
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
			(config, index) => new DestinationRow(index, _($"Replace #{index + 1}: {config.Name}"))
		);
		_destinationCombo = new CustomComboRow<DestinationRow>(_destination)
		{
			LabelCallback = destination => destination.Name,
			Items = new[] { new DestinationRow(null, _("New preset")) }
				.Concat(existingPresetOptions),
		};
	}

	private void Validate()
	{
		var isValid = _name.TextLength > 0;
		SetResponseEnabled("save", isValid);
	}

	private void Save()
	{
		var destination = _destinationCombo.SelectedItem;
		_presets.SaveCurrent(
			_camera, 
			_name.Text_ ?? string.Empty, 
			index: destination?.Index
		);
		Close();
	}

	private record DestinationRow(
		int? Index,
		string Name
	);
}
