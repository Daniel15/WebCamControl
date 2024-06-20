namespace WebCamControl.Core;

/// <summary>
/// Handles persisting config file to disk.
/// </summary>
public interface IConfigManager
{
	/// <summary>
	/// Saves the current config to disk.
	/// </summary>
	void Save();
}
