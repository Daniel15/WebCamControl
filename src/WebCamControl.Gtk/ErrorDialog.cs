// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using Gtk;
using static WebCamControl.Core.Gettext;

namespace WebCamControl.Gtk;

public class ErrorDialog : Adw.AlertDialog
{
	[Connect] private readonly TextView _details = default!;
	[Connect] private readonly Label _summary = default!;
	
	public ErrorDialog(Exception ex)
		: this(ex, new Builder("ErrorDialog.ui")) { }

	private ErrorDialog(Exception ex, Builder builder)
		: base(builder.GetPointer("error_dialog"), false)
	{
		builder.Connect(this);
		_summary.Label_ = _($"If this is unexpected, please report a bug. Error: {ex.Message}");
		_details.Buffer!.Text = ex.ToString();
	}
}
