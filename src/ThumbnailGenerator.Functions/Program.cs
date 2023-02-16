using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Text.Json.Serialization;

JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
{
	PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
	DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
	DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
};

string? appConfigConnectionString = Environment.GetEnvironmentVariable("AppConfigConnectionString");

var host = new HostBuilder()
				.ConfigureAppConfiguration(builder =>
				{
					string cs = Environment.GetEnvironmentVariable("AppConfigConnectionString");
					builder.AddAzureAppConfiguration(cs);
				}).ConfigureFunctionsWorkerDefaults()
	.ConfigureServices(s =>
	{
		s.AddSingleton((s) => { return jsonSerializerOptions; });
	})
	.Build();

host.Run();