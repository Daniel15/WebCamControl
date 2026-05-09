// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using Adw;
using Gtk;
using GObject;
using Microsoft.Extensions.DependencyInjection;
using WebCamControl.Core;
using WebCamControl.Gtk.Widgets;
using static WebCamControl.Core.Gettext;

namespace WebCamControl.Gtk;

[Subclass<Adw.AlertDialog>(qualifiedName: nameof(SavePresetDialog))]
[Template<EntryAssemblyResource>("SavePresetDialog.ui")]
public partial class SavePresetDialog : IWidgetWithServiceLocator<SavePresetDialog>
{
	private const string _changedSignalName = "changed";
	private static readonly Signal<EntryRow> _changedSignal =
		new(_changedSignalName, _changedSignalName);

	private ICamera _camera = null!;
	private IPresets _presets = null!;
	private CustomComboRow<DestinationRow> _destinationCombo = null!;
	
	[Connect] private EntryRow _name;
	[Connect] private ComboRow _destination;
	
	public static SavePresetDialog New(IServiceProvider provider)
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
		_presets.SaveCurrent(
			_camera, 
			_name.Text_ ?? string.Empty, 
			index: _destinationCombo.SelectedItem?.Index
		);
		Close();
	}

	private record DestinationRow(
		int? Index,
		string Label
	)
	{
		public override string ToString() => Label;
	};
}
