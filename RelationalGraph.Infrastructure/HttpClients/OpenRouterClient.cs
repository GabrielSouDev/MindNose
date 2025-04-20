using Microsoft.Extensions.Configuration;
using RelationalGraph.Application.Interfaces.Clients;
using RelationalGraph.Application.Operations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
namespace RelationalGraph.Infrastructure.HttpClients
{
    public class OpenRouterClient : IOpenRouterClient
    {
        private readonly HttpClient _httpClient;

        public OpenRouterClient(HttpClient httpClient, IConfigurationSection openRouterConfig)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            if (openRouterConfig == null)
                throw new ArgumentNullException(nameof(openRouterConfig));

            string? projectTitle = openRouterConfig["ProjectTitle"];
            string? apiKey = openRouterConfig["ApiKey"];
            string? url = openRouterConfig["Url"];
            string? site = openRouterConfig["Site"];

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(url) || string.IsNullOrEmpty(site))
                throw new ArgumentException(nameof(apiKey), "cannot be null or empty.");

            _httpClient = httpClient;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(url)
            };

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", site);
            _httpClient.DefaultRequestHeaders.Add("X-Title", projectTitle);
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
                temperature = 0.7
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