// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

using System.Runtime.InteropServices;
using WebCamControl.Core;
using WebCamControl.Core.Exceptions;

namespace WebCamControl.Linux.Interop;

public static class NativeGettext
{
	[DllImport("libc", SetLastError = true, CharSet = CharSet.Ansi)]
	public static extern IntPtr setlocale(LocaleCategory category, string locale);

	[DllImport("libc", SetLastError = true, CharSet = CharSet.Ansi)]
	public static extern IntPtr bindtextdomain(string domain, string dir);

	[DllImport("libc", SetLastError = true, CharSet = CharSet.Ansi)]
	public static extern IntPtr textdomain(string domain);

	/// <summary>
	/// Initialize native gettext. This has to be done early in the app initialization, before any
	/// Gtk components are created.
	/// </summary>
	public static void Init()
	{
		InteropException.ThrowIfError(setlocale(LocaleCategory.All, ""));
		/*
		 * TODO: Set correct path here. Need to figure out the best way of distributing the .mo
		 * files. Maybe embed them as resources and extract them to a temp directory? For 
		 * packages or Flatpak, they could go in the proper place (/usr/share/locale).
		 *
		 * Files need to be in ...../[locale]/LC_MESSAGES/webcamcontrol.mo
		 */
		// InteropException.ThrowIfError(
		// 	bindtextdomain(Gettext.TextDomain, "/home/daniel/src/WebCamControl/po/test")
		// );
		InteropException.ThrowIfError(textdomain(Gettext.TextDomain));
	}
}
