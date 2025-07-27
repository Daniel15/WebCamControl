// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

using System.Globalization;
using System.Reflection;
using GetText;
using GetText.Loaders;

namespace WebCamControl.Core;

public static class Gettext
{
	public const string TextDomain = "webcamcontrol";
	
	// TODO: See if dependency injection can be used for this.
	private static readonly ICatalog _catalog;

	static Gettext()
	{
		var cultureInfo = CultureInfo.CurrentUICulture;
		var (localeDir, localeFile) = FindLocaleFile(cultureInfo);
		LocaleDirectory = localeDir;
		
		if (cultureInfo.TwoLetterISOLanguageName != "en")
		{
			if (localeFile != null)
			{
				Console.WriteLine($"Found translations for {cultureInfo.TwoLetterISOLanguageName}");
				_catalog = new Catalog(new MoLoader(localeFile), cultureInfo);
				return;
			}
			Console.WriteLine(
				$"No translations for {cultureInfo.TwoLetterISOLanguageName}; falling back to English."
			);
		}
		_catalog = new Catalog();
	}

	private static (string?, string?) FindLocaleFile(CultureInfo cultureInfo)
	{
		// Directories to look for .mo files in
		var potentialDirectories = new[]
		{
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Locales"),
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../Locales"),
			"/usr/share/locale"
		};
		foreach (var directory in potentialDirectories)
		{
			var fullPath = Path.Combine(directory, GetLocaleRelativePath(cultureInfo));
			if (File.Exists(fullPath))
			{
				return (directory, fullPath);
			}
		}
		return (null, null);
	}

	private static string GetLocaleRelativePath(CultureInfo cultureInfo)
	{
		return Path.Combine(
			cultureInfo.TwoLetterISOLanguageName,
			"LC_MESSAGES",
			"webcamcontrol.mo"
		);
	}

	public static string? LocaleDirectory { get; }

	public static string _(FormattableString text)
	{
		return _catalog.GetString(text);
	}

	public static string _(FormattableStringAdapter text)
	{
		return _catalog.GetString(text);
	}
}
