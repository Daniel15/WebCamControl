// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
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
		_summary.Label_ = _($"Error: {ex.Message}\n\nIf this is unexpected, please report a bug.");
		_details.Buffer!.Text = ex.ToString();
	}

	public static void ShowError(
		Exception ex,
		Adw.Application app,
		Widget? parent
	)
	{
		var dialog = new ErrorDialog(ex);
		dialog.OnResponse += (_, args) =>
		{
			Console.WriteLine(args.Response);
			if (args.Response == "report_bug")
			{
				ReportBug(ex);
			}
			app.Release();
			app.Quit();
		};
		// .Hold() ensures the app does not close until the dialog is closed
		app.Hold();
		dialog.Present(parent);
	}

	private static void ReportBug(Exception ex)
	{
		Process.Start(new ProcessStartInfo
		{
			FileName = BugReport.BuildBugReportUri(ex).ToString(),
			UseShellExecute = true,
		});
		// HACK! The Process.Start doesn't seem to work if the app immediately exits afterwards. 
		// Wait a bit before exiting.
		Thread.Sleep(500);
	}
}
