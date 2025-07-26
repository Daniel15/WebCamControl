// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

namespace WebCamControl.Core.Configuration;

/// <summary>
/// Contains top-level configuration for the app.
/// </summary>
public class Config
{
	/// <summary>
	/// Presets that are available for use.
	/// </summary>
	public IList<PresetConfig> Presets { get; set; } = new List<PresetConfig>();
	
	/// <summary>
	/// Name of the camera that has been selected in the UI. This stores the camera's friendly name
	/// (e.g. "Insta360 Link") rather than the device name (e.g. "video0") because device
	/// enumeration order is undefined and can change between reboots. 
	/// </summary>
	public string? SelectedCamera { get; set; }
}
