using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RelationalGraph.Application.Interfaces.Clients;
using RelationalGraph.Application.Operations;
using RelationalGraph.Domain.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
namespace RelationalGraph.Infrastructure.HttpClients
{
    public class OpenRouterClient : IOpenRouterClient
    {
        private readonly HttpClient _httpClient;
        public readonly OpenRouterSettings _settings;

        public OpenRouterClient(HttpClient httpClient, IOptions<OpenRouterSettings> settings)
        {
            _settings = settings.Value;

            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            if (_settings == null)
                throw new ArgumentNullException(nameof(_settings), "cannot be null or empty.");

            _httpClient = httpClient;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_settings.Url)
            };

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.ApiKey);

            _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", _settings.site);
            _httpClient.DefaultRequestHeaders.Add("X-Title", _settings.ProjectTitle);
        }

        public async Task<string> EnviarPrompt(Prompt prompt)
        {
            var body = new
            {
                model = "meta-llama/llama-3.3-70b-instruct:free",
                messages = new[]
                {
                new { role = "user", content = prompt.Message }
            },
                temperature = 0.0
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("chat/completions", content);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            catch (HttpRequestException e)
            {
                throw new ApplicationException("Error sending request to OpenRouter API, ", e);
            }
        }
    }
}