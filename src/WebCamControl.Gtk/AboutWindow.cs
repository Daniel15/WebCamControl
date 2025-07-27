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
		var dialog = new AboutDialog
		{
			ApplicationIcon = "icon-512x512",
			ApplicationName = "WebCamControl",
			Copyright = "© 2024-2025 Daniel Lo Nigro (Daniel15)",
			DeveloperName = "Daniel Lo Nigro",
			Developers = ["Daniel Lo Nigro <apps+wcc@d.sb>"],
			IssueUrl = BugReport.BuildBugReportUri(null).ToString(),
			LicenseType = License.MitX11,
			SupportUrl = "https://github.com/Daniel15/WebCamControl/issues",
			TranslatorCredits = _("translator-credits"),
			Version = Assembly.GetEntryAssembly() ?.GetName().Version?.ToString() ?? "Unknown",
			Website = "https://d.sb/wcc",
		};
		dialog.Present(parent);
	}
}
