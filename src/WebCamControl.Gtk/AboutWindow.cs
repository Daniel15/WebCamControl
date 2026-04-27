// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

using System.Reflection;
using Adw;
using Gtk;
using AboutDialog = Adw.AboutDialog;
using static WebCamControl.Core.Gettext;

namespace WebCamControl.Gtk;

public static class AboutWindow
{
	public static void Show(Widget? parent)
	{
		var dialog = AboutDialog.NewWithProperties([]);
		dialog.ApplicationIcon = "icon-512x512";
		dialog.ApplicationName = "WebCamControl";
		dialog.Copyright = "© 2024-2025 Daniel Lo Nigro (Daniel15)";
		dialog.DeveloperName = "Daniel Lo Nigro";
		dialog.Developers = ["Daniel Lo Nigro <apps+wcc@d.sb>"];
		dialog.IssueUrl = BugReport.BuildBugReportUri(null).ToString();
		dialog.LicenseType = License.MitX11;
		dialog.SupportUrl = "https://github.com/Daniel15/WebCamControl/issues";
		dialog.TranslatorCredits = _("translator-credits");
		dialog.Version = Assembly.GetEntryAssembly() ?.GetName().Version?.ToString() ?? "Unknown";
		dialog.Website = "https://d.sb/wcc";
		dialog.Present(parent);
	}
}
