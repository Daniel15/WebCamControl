// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2024 Daniel Lo Nigro <d@d.sb>

using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using Gtk;
using WebCamControl.Gtk;
using static WebCamControl.Core.Gettext;

namespace WebCamControl.GtkViews;

[GObject.Subclass<Adw.AlertDialog>(qualifiedName: nameof(ErrorDialog))]
[global::Gtk.Template<global::Gtk.AssemblyResource>("ErrorDialog.ui")]
public partial class ErrorDialog
{
	[Connect] private TextView _details;
	[Connect] private Label _summary;
	
	public static ErrorDialog Create(Exception ex)
	{
		var dialog = NewWithProperties([]);
		dialog.Configure(ex);
		return dialog;
	}

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
		var dialog = Create(ex);
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
