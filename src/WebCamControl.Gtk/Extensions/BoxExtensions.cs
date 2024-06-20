using Gtk;

namespace WebCamControl.Gtk.Extensions;

/// <summary>
/// Extension methods for <see cref="Gtk.Box"/>.
/// </summary>
public static class BoxExtensions
{
	/// <summary>
	/// Removes all children in this box.
	/// </summary>
	public static void RemoveChildren(this Box box)
	{
		var child = box.GetLastChild();
		while (child != null)
		{
			box.Remove(child);
			child = box.GetLastChild();
		}
	}
}
