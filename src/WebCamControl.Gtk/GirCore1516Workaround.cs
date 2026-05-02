// Temporary hacks until https://github.com/gircore/gir.core/pull/1516 lands.
// Wrappers around Gtk library to avoid namespace issues.

namespace WebCamControl.Gtk.Internal;

internal class WidgetClassUnownedHandle(nint handle) : global::Gtk.Internal.WidgetClassUnownedHandle(handle);

internal static class WidgetClass
{
	public static void SetTemplate(
		WidgetClassUnownedHandle widgetClass,
		GLib.Internal.BytesHandle templateBytes
	) => global::Gtk.Internal.WidgetClass.SetTemplate(widgetClass, templateBytes);

	public static void BindTemplateChildFull(
		WidgetClassUnownedHandle widgetClass,
		GLib.Internal.NonNullableUtf8StringOwnedHandle name,
		bool internalChild,
		nint structOffset
	) => global::Gtk.Internal.WidgetClass.BindTemplateChildFull(widgetClass, name, internalChild, structOffset);
}

internal static class Widget
{
	public static void InitTemplate(nint instance) => global::Gtk.Internal.Widget.InitTemplate(instance);

	public static void DisposeTemplate(nint instance, GObject.Type gType) =>
		global::Gtk.Internal.Widget.DisposeTemplate(instance, gType);
}
