using Microsoft.Extensions.Configuration;

namespace TaleLearnCode.ThumbnailGenerator.Extensions;

internal static class ConfigurationExtensions
{

	internal static int GetIntValue(this IConfigurationRoot config, string key)
		=> int.TryParse(config[key], out int value) ? value : throw new InvalidCastException();

	internal static bool GetBoolValue(this IConfigurationRoot config, string key)
		=> bool.TryParse(config[key], out bool value) ? value : throw new InvalidCastException();

}