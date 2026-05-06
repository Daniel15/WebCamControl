// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>
// https://d.sb/wcc

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// Represents an item in a list widget.
/// </summary>
public interface IListItem
{
	/// <summary>
	/// The label to display for this item in the UI.
	/// </summary>
	public string Label { get; }
}
