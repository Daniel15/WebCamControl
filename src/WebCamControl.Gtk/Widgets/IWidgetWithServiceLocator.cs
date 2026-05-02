// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using Gtk;

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// Represents a <see cref="Widget"/> that is created using a service provider.
/// </summary>
public interface IWidgetWithServiceLocator<out T> where T : IWidgetWithServiceLocator<T> 
{
	public static abstract T New(IServiceProvider provider);
}
