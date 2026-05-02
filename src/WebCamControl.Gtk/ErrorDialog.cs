// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using System.Diagnostics;
using Gtk;
using static WebCamControl.Core.Gettext;

namespace WebCamControl.Gtk;

[GObject.Subclass<Adw.AlertDialog>(qualifiedName: nameof(ErrorDialog))]
[Template<EntryAssemblyResource>("ErrorDialog.ui")]
public partial class ErrorDialog
{
	[Connect] private TextView _details;
	[Connect] private Label _summary;

	private void Configure(Exception ex)
	{
		_summary.Label_ = _($"Error: {ex.Message}\n\nIf this is unexpected, please report a bug.");
		_details.Buffer!.Text = ex.ToString();
	}

	public static void ShowError(
		Exception ex,
		Adw.Application app,
		Widget? parent
	)
	{
		var dialog = NewWithProperties([]);
		dialog.Configure(ex);
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
