using Adw;
using Gtk;
using WebCamControl.Core.Configuration;
using MessageDialog = Adw.MessageDialog;

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// A row representing a saved preset.
/// </summary>
public class PresetRow : ExpanderRow
{
	private readonly PresetConfig _preset;
	public event EventHandler? OnDelete; 
		
	public PresetRow(PresetConfig preset)
		: this(Adw.Internal.ExpanderRow.New(), false, preset)
	{
		
	}
	private PresetRow(IntPtr ptr, bool ownedRef, PresetConfig preset)
		: base(ptr, ownedRef)
	{
		_preset = preset;
		Title = preset.Name;
			
		if (preset.Tilt != null)
		{
			var tilt = ActionRow.New();
			tilt.Title = $"Tilt: {Math.Round((decimal)preset.Tilt.Value, decimals: 2)}";
			AddRow(tilt);
		}
		if (preset.Pan != null)
		{
			var pan = ActionRow.New();
			pan.Title = $"Pan: {Math.Round((decimal)preset.Pan.Value, decimals: 2)}";
			AddRow(pan);
		}

		var deleteButton = Button.New();
		deleteButton.TooltipText = $"Delete {preset.Name}";
		deleteButton.IconName = "user-trash-symbolic";
		deleteButton.CssClasses = ["flat", "image-button"];
		deleteButton.OnClicked += (_, _) => ConfirmDelete();
		AddSuffix(deleteButton);
	}

	private void ConfirmDelete()
	{
		const string deleteButtonId = "delete";
		const string cancelButtonId = "cancel";
		
		var dialog = new MessageDialog();
		dialog.SetParent(this);
		dialog.Body = $"Are you sure you want to delete preset '{_preset.Name}'?";
		dialog.AddResponse(cancelButtonId, "Cancel");
		dialog.AddResponse(deleteButtonId, "Delete");
		dialog.SetResponseAppearance(deleteButtonId, ResponseAppearance.Destructive);
		dialog.SetDefaultResponse(cancelButtonId);
		dialog.SetCloseResponse(cancelButtonId);
		dialog.OnResponse += (_, args) =>
		{
			if (args.Response == deleteButtonId)
			{
				OnDelete?.Invoke(this, EventArgs.Empty);
			}
		};
		dialog.Present();
	}
}
