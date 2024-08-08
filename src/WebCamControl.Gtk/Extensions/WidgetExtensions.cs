using Gtk;

namespace WebCamControl.Gtk.Extensions;

/// <summary>
/// Extensions methods for <see cref="Widget"/>
/// </summary>
internal static class WidgetExtensions
{
	public static void DisableCameraControlIfUnsupported(this Widget widget, object? cameraControl)
	{
		var isSupported = cameraControl != null;
		widget.Sensitive = isSupported;
		widget.TooltipText = isSupported 
			? null 
			: "Not supported by this camera";
	}
}
