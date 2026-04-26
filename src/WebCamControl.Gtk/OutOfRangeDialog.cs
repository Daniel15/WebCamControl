// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using System.Text;
using Adw;
using Gtk;
using WebCamControl.Core;
using WebCamControl.Gtk.Extensions;
using static WebCamControl.Core.Gettext;

namespace WebCamControl.Gtk;

public static class OutOfRangeDialog
{
	private const string ResetResponse = "reset";
	private const string IgnoreResponse = "ignore";
	private const string OKResponse = "okay";

	public static void Show(
		IList<OutOfRangeDetector.Result> outOfRangeControls,
		Widget? parent
	)
	{
		var body = new StringBuilder();
		body.AppendLine(_(
			"Your camera has control values that are out of range. This is a bug with the " +
			"camera's firmware or driver. To try and recover, WebCamControl can attempt to reset " +
			"these controls to their default values."
		));
		body.AppendLine();
		
		foreach (var (name, control) in outOfRangeControls)
		{
			body.AppendLine(_(
				$"{name}: Set to {control.Value} but should be between {control.Minimum} and {control.Maximum}."
			));
		}
		
		var bodyLabel = Label.New(body.ToString());
		bodyLabel.Wrap = true;
		bodyLabel.Halign = Align.Start;
		bodyLabel.Justify = Justification.Left;

		var dialog = Adw.AlertDialog.NewWithLeftAlignedText(
			_("Out of range values"), 
			body.ToString()
		);
		dialog.WidthRequest = 600;
		dialog.AddResponse(IgnoreResponse, _("Ignore"));
		dialog.AddResponse(ResetResponse, _("Reset"));
		dialog.CloseResponse = IgnoreResponse;
		dialog.DefaultResponse = ResetResponse;
		dialog.SetResponseAppearance(ResetResponse, ResponseAppearance.Suggested);
		
		dialog.OnResponse += (_, args) =>
		{
			if (args.Response == ResetResponse)
			{
				Reset(outOfRangeControls, dialog);
			}
			dialog.Close();
		};
		dialog.Present(parent);
	}

	private static void Reset(
		IEnumerable<OutOfRangeDetector.Result> outOfRangeControls,
		Widget parent
	)
	{
		var errors = new List<string>();
		foreach (var (name, control) in outOfRangeControls)
		{
			try
			{
				control.Reset();
			}
			catch (Exception ex)
			{
				errors.Add($"{name}\n{ex.Message}");
			}
		}

		if (errors.Count == 0)
		{
			return;
		}

		var body = 
			_("Some controls could not be reset:") + "\n\n" + string.Join("\n\n", errors);
		var dialog = Adw.AlertDialog.NewWithLeftAlignedText(
			_("An error has occurred"),
			body
		);
		dialog.WidthRequest = 600;
		dialog.AddResponse(OKResponse, _("OK"));
		dialog.OnResponse += (_, args) => dialog.Close();
		dialog.Present(parent);
	}
}
