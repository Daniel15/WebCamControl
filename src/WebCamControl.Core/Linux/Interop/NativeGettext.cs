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
		var localeDirectory = Gettext.LocaleDirectory;
		if (localeDirectory == null)
		{
			return;
		}
	
		InteropException.ThrowIfError(setlocale(LocaleCategory.All, ""));
		InteropException.ThrowIfError(
			bindtextdomain(Gettext.TextDomain, localeDirectory)
		);
		InteropException.ThrowIfError(textdomain(Gettext.TextDomain));
	}
}
