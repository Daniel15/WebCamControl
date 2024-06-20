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
}
