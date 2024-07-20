using Microsoft.Extensions.Options;
using WebCamControl.Core.Configuration;

namespace WebCamControl.Core;

/// <summary>
/// Handles loading, applying, and saving camera presets.
/// </summary>
public interface IPresets
{
	/// <summary>
	/// Fired when any presets are changed
	/// </summary>
	public event EventHandler? OnChange;
	
	/// <summary>
	/// Gets a list of all available presets.
	/// </summary>
	public IList<PresetConfig> PresetConfigs { get; }

	/// <summary>
	/// Applies the specified preset to the specified camera.
	/// </summary>
	/// <param name="preset">Preset to apply</param>
	/// <param name="camera">Camera to apply it to</param>
	public void ApplyTo(PresetConfig preset, ICamera camera);

	/// <summary>
	/// Saves the camera's current settings as a preset.
	/// </summary>
	/// <param name="index">Preset index to overwrite. If <c>null</c>, a new preset will be saved.</param>
	public void SaveCurrent(ICamera camera, string name, int? index);
}

