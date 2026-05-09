// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// Represents a widget that contains a list of items.
/// </summary>
public interface IListWidget<T> where T : notnull
{
	/// <summary>
	/// Occurs when the selected item is changed.
	/// </summary>
	public event EventHandler<T?>? SelectionChanged;
	
	/// <summary>
	/// Gets or sets the list of items to show 
	/// </summary>
	public IEnumerable<T> Items { get; set; }
	
	/// <summary>
	/// Gets or sets the currently selected item.
	/// </summary>
	public T? SelectedItem { get; set; }
}
