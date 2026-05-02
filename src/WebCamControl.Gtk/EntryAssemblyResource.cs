// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using System.Reflection;
using GLib;
using GObject;
using Gtk;

namespace WebCamControl.Gtk;

/// <summary>
/// Like <see cref="AssemblyResource"/> but loads from the entry assembly
/// rather than the calling assembly. This is required to avoid issues
/// when running a single-file build in release mode.
/// https://github.com/Daniel15/WebCamControl/issues/39 
/// </summary>
public class EntryAssemblyResource : TemplateLoader
{
	public static Bytes Load(string resourceName)
	{
		var data = Assembly.GetEntryAssembly()!.ReadResourceAsByteArray(resourceName);
		return Bytes.New(data);
	}
}
