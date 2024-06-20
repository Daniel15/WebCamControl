using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebCamControl.Core.Configuration;

namespace WebCamControl.Core;

/// <summary>
/// Handles persisting config file to disk.
/// </summary>
public class ConfigManager : IConfigManager
{
	private readonly ILogger<ConfigManager> _logger;
	private readonly IOptionsMonitor<Config> _config;

	private static readonly JsonSerializerOptions _jsonOptions = new()
	{
		WriteIndented = true,
	};

	public ConfigManager(ILogger<ConfigManager> logger, IOptionsMonitor<Config> config)
	{
		_logger = logger;
		_config = config;
		
		_logger.LogInformation("Config path: {ConfigPath}", ConfigPath);
	}

	/// <summary>
	/// Saves the current config to disk.
	/// </summary>
	public void Save()
	{
		var newConfig = JsonSerializer.Serialize(_config.CurrentValue, _jsonOptions);
		_logger.LogInformation("Saving config");
		File.WriteAllText(ConfigPath, newConfig);
	}

	/// <summary>
	/// Gets the path to the configuration file.
	/// </summary>
	public static string ConfigPath => Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
		"webcamcontrol.json"
	);
}
