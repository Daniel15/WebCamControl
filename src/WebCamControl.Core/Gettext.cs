// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

using GetText;

namespace WebCamControl.Core;

public static class Gettext
{
	public const string TextDomain = "webcamcontrol";
	
	// TODO: See if dependency injection can be used for this.
	private static readonly ICatalog _catalog;

	static Gettext()
	{
		// TODO: Initialize native gettext for translations in Blueprint files
		// TODO: Load correct .mo file
		//var moStream = File.OpenRead("/home/daniel/src/WebCamControl/po/test/en/LC_MESSAGES/webcamcontrol.mo");
		//_catalog = new Catalog(moStream, new CultureInfo("en-US"));
		_catalog = new Catalog();
	}
	
	public static string _(FormattableString text)
	{
		return _catalog.GetString(text);
	}

	public static string _(FormattableStringAdapter text)
	{
		return _catalog.GetString(text);
	}
}
