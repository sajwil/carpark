using Microsoft.Extensions.Configuration;

namespace Emprevo.IntegrationTests.Extenstion
{
    public static class ConfigurationProvider
    {
        public static IConfiguration GetConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            return configuration;
        }
    }
}
