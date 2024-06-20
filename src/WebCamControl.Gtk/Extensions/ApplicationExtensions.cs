using Gio;
using GObject;

namespace WebCamControl.Gtk.Extensions;

/// <summary>
/// Extension methods for <see cref="Adw.Application"/>.
/// </summary>
public static class ApplicationExtensions
{
	/// <summary>
	/// Handles boilerplate for configuring a menu item.
	/// </summary>
	public static void ConfigureMenuItem(
		this Adw.Application app,
		string name,
		SignalHandler<SimpleAction, SimpleAction.ActivateSignalArgs> handler,
		string? accelerator = null
	)
	{
		var actionItem = SimpleAction.New(name, null);
		actionItem.OnActivate += handler;
		app.AddAction(actionItem);
		if (accelerator != null)
		{
			app.SetAccelsForAction($"app.{name}", [accelerator]);
		}
	}
}
