using RelationalGraph.Application.Interfaces.Services;
using RelationalGraph.Domain.LLMModels;
using System.Text.Json;
namespace RelationalGraph.Application.Services
{
    public class ModelsStorageService : IModelsStorageService
    {
        public async Task<ModelResponse> LoadModelsJson()
        {
            var projectRoot = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.FullName;
            var path = Path.Combine(projectRoot, "RelationalGraph.Infrastructure", "Storage", "models.json");

            if (File.Exists(path))
            {
                var json = await File.ReadAllTextAsync(path);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<ModelResponse>(json, options);
            }

            return null;
        }

        public async Task UpdateModelsJson()
        {
            string response = string.Empty;
            using (var httpClient = new HttpClient())
                response = await httpClient.GetStringAsync("https://openrouter.ai/api/v1/models");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var Models = JsonSerializer.Deserialize<ModelResponse>(response, options);

            var projectRoot = Directory.GetParent(Environment.CurrentDirectory)?.FullName;
            var path = Path.Combine(projectRoot, "RelationalGraph.Infrastructure", "Storage", "models.json");

            File.WriteAllText(path, JsonSerializer.Serialize(Models, options));
        }
    }
}
