namespace WebCamControl.Core.Configuration;

/// <summary>
/// Configuration for one preset
/// </summary>
public record PresetConfig(
	string Name,
	float? Pan,
	float? Tilt,
	int? Zoom
);
