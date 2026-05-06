// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>
// https://d.sb/wcc

using Gio;
using WebCamControl.Gtk.Widgets;

namespace WebCamControl.Gtk.Extensions;

public static class MenuExtensions
{
	/// <summary>
	/// Appends the given submenu to the menu.
	/// </summary>
	public static void AppendSubmenu<T>(
		this Menu menu,
		string label,
		RadioButtonSubmenu<T> submenu
	) where T : IListItem
	{
		menu.AppendSubmenu(label, submenu.Menu);
	}
}
