// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

using Gtk;

namespace WebCamControl.Gtk.Extensions;

/// <summary>
/// Extensions methods for <see cref="Widget"/>
/// </summary>
internal static class WidgetExtensions
{
	public static void DisableCameraControlIfUnsupported(this Widget widget,
		object? cameraControl)
	{
		var isSupported = cameraControl != null;
		widget.Sensitive = isSupported;
		widget.TooltipText = isSupported
			? null
			: "Not supported by this camera";
	}

	/// <summary>
	/// Get an enumerator containing the children of the specified widget.
	/// </summary>
	public static IEnumerable<Widget> GetChildren(this Widget widget)
	{
		// This has to return a list rather than just yielding the children, since some call sites
		// use it to delete children.
		var children = new List<Widget>();
		var child = widget.GetFirstChild();
		while (child != null)
		{
			children.Add(child);
			child = child.GetNextSibling();
		}

		return children;
	}

	/// <summary>
	/// Removes all children of this widget.
	/// </summary>
	public static void RemoveChildren(this Widget widget)
	{
		var child = widget.GetLastChild();
		while (child != null)
		{
			// This is a mess, but GirCore doesn't have an interface for it :/
			switch (widget)
			{
				case Box box:
					box.Remove(child);
					break;
				case ListBox listBox:
					listBox.Remove(child);
					break;
				default:
					throw new ArgumentException($"Widget type {widget.GetType()} not supported");
			}
			
			child = widget.GetLastChild();
		}
	}
}
