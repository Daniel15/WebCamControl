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
	}

	/// <summary>
	/// Saves the camera's current settings as a preset.
	/// </summary>
	public void SaveCurrent(ICamera camera, string name)
	{
		var newPreset = new PresetConfig(
			Name: name,
			Pan: camera.Pan?.Value,
			Tilt: camera.Tilt?.Value
		);
		PresetConfigs.Add(newPreset);
		_configManager.Save();
		OnChange?.Invoke(this, EventArgs.Empty);
	}
}
