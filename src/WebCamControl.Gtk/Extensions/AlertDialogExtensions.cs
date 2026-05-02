// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using Gtk;

namespace WebCamControl.Gtk.Extensions;

public static class AlertDialogExtensions
{
	extension(Adw.AlertDialog) {
		/// <summary>
		/// For dialogs with a lot of text, where the default center alignment looks ugly.
		/// </summary>
		public static Adw.AlertDialog NewWithLeftAlignedText(string heading, string body)
		{
			var bodyLabel = Label.New(body);
			bodyLabel.Wrap = true;
			bodyLabel.Halign = Align.Start;
			bodyLabel.Justify = Justification.Left;

			var dialog = Adw.AlertDialog.New(heading, null);
			dialog.ExtraChild = bodyLabel;
			return dialog;
		} 
	}
}
