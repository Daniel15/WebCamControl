using Microsoft.Extensions.Options;
using WebCamControl.Core.Configuration;

namespace WebCamControl.Core;

/// <summary>
/// Handles loading, applying, and saving camera presets.
/// </summary>
public class Presets : IPresets
{
	private readonly IOptionsMonitor<Config> _config;
	private readonly IConfigManager _configManager;

	public Presets(IOptionsMonitor<Config> config, IConfigManager configManager)
	{
		_config = config;
		_configManager = configManager;
	}
	
	/// <summary>
	/// Fired when any presets are changed
	/// </summary>
	public event EventHandler? OnChange;

	/// <summary>
	/// Gets a list of all available presets.
	/// </summary>
	public IList<PresetConfig> PresetConfigs => _config.CurrentValue.Presets;

	/// <summary>
	/// Applies the specified preset to the specified camera
	/// </summary>
	/// <param name="preset">Preset to apply</param>
	/// <param name="camera">Camera to apply it to</param>
	public void ApplyTo(PresetConfig preset, ICamera camera)
	{
		if (camera.Pan != null && preset.Pan != null)
		{
			camera.Pan.Value = preset.Pan.Value;	
		}

		if (camera.Tilt != null && preset.Tilt != null)
		{
			camera.Tilt.Value = preset.Tilt.Value;	
		}

		if (camera.Zoom != null && preset.Zoom != null)
		{
			camera.Zoom.Value = preset.Zoom.Value;
		}
 	}

	/// <summary>
	/// Saves the camera's current settings as a preset.
	/// </summary>
	public void SaveCurrent(ICamera camera, string name, int? index)
	{
		var newPreset = new PresetConfig(
			Name: name,
			Pan: camera.Pan?.Value,
			Tilt: camera.Tilt?.Value,
			Zoom: camera.Zoom?.Value
		);
		if (index == null)
		{
			PresetConfigs.Add(newPreset);			
		}
		else
		{
			PresetConfigs[index.Value] = newPreset;
		}
		
		_configManager.Save();
		OnChange?.Invoke(this, EventArgs.Empty);
	}

	/// <summary>
	/// Deletes the preset at the specified index.
	/// </summary>
	public void Delete(PresetConfig preset)
	{
		PresetConfigs.Remove(preset);
		_configManager.Save();
		OnChange?.Invoke(this, EventArgs.Empty);
	}
}
