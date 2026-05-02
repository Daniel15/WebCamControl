// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using Gtk;

namespace WebCamControl.Gtk;

/// <summary>
/// Represents an <see cref="Adw.Window"/> that is created using a service provider.
/// </summary>
public interface IWindow<out T> where T : Window, IWindow<T> 
{
	public static abstract T New(IServiceProvider provider);
}
