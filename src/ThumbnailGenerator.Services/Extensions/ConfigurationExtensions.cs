namespace TaleLearnCode.ThumbnailGenerator.Extensions;

internal static class ConfigurationExtensions
{

	internal static int GetIntValue(this IConfiguration config, string key)
		=> int.TryParse(config[key], out int value) ? value : throw new InvalidCastException();

	internal static bool GetBoolValue(this IConfiguration config, string key)
		=> bool.TryParse(config[key], out bool value) ? value : throw new InvalidCastException();

}