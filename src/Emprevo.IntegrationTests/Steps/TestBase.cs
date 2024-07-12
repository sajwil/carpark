using Emprevo.IntegrationTests.Extenstion;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Xunit.Gherkin.Quick;

namespace Emprevo.IntegrationTests.Steps
{
    public abstract class TestBase : Feature
    {
        private readonly HttpClient _httpClient;

        protected TestBase()
        {
            var configuration = ConfigurationProvider.GetConfiguration();
            var uriString = configuration["AppSettings:BaseUri"] ?? throw new ArgumentException("BaseUri required");
            _httpClient = new HttpClient
            {

                BaseAddress = new Uri(string.Format("{0}/v1/Calculation", uriString))
            };
        }

        public async Task<T> GetDataFromApi<T>(Dictionary<string, DateTime> input) where T : new()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, string.Empty);

            var content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, MediaTypeNames.Application.Json);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var deserializedData = JsonSerializer.Deserialize<T>(responseBody);
            return deserializedData ?? new T();
        }
    }
}