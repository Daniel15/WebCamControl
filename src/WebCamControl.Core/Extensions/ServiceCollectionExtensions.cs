using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebCamControl.Core.Configuration;
using WebCamControl.Core.Linux;

namespace WebCamControl.Core.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddWebCamControl(this IServiceCollection services)
	{
		var config = new ConfigurationBuilder()
			.AddJsonFile(ConfigManager.ConfigPath, optional: true, reloadOnChange: true)
			.Build();

		return services
			.Configure<Config>(config)
			.AddSingleton<ICameraManager, LinuxCameraManager>()
			.AddSingleton<IConfigManager, ConfigManager>()
			.AddSingleton<IPresets, Presets>();
	}
}
