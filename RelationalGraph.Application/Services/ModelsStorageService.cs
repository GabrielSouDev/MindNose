using RelationalGraph.Application.Interfaces.Services;
using RelationalGraph.Domain.LLMModels;
using System.Text.Json;
namespace RelationalGraph.Application.Services
{
    public class ModelsStorageService : IModelsStorageService
    {
        public async Task<ModelResponse?> LoadModelsJson()
        {
            var baseDir = AppContext.BaseDirectory;
            var path = Path.Combine(baseDir, "Storage", "models.json");


            if (File.Exists(path))
            {
                var json = await File.ReadAllTextAsync(path);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<ModelResponse>(json, options);
            }
            else
                return await UpdateModelsJson();
        }

        public async Task<ModelResponse?> UpdateModelsJson()
        {
            string response = string.Empty;
            using (var httpClient = new HttpClient())
                response = await httpClient.GetStringAsync("https://openrouter.ai/api/v1/models");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var Models = JsonSerializer.Deserialize<ModelResponse>(response, options);

            var baseDir = AppContext.BaseDirectory;
            var basePath = Path.Combine(baseDir, "Storage");

            if (Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            var path = Path.Combine(baseDir, "models.json");
            File.WriteAllText(path, JsonSerializer.Serialize(Models, options));

            return Models;
        }
    }
}
