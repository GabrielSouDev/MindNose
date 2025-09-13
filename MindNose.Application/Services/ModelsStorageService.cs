using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.LLMModels;
using System.Text.Json;
namespace MindNose.Domain.Services;

public class ModelsStorageService : IModelsStorageService
{
    private ModelResponse? Models = new();
    public async Task InitializeAsync()
    {
        Models = await LoadModelsJsonAsync();

    }

    private async Task<ModelResponse?> LoadModelsJsonAsync()
    {
        var baseDir = AppContext.BaseDirectory;
        var path = Path.Combine(baseDir, "Storage", "models.json");


        if (File.Exists(path))
        {
            var json = await File.ReadAllTextAsync(path);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            Console.WriteLine("- Lista de Modelos LLM Carregada!");
            return JsonSerializer.Deserialize<ModelResponse>(json, options);
        }
        else
            return await UpdateModelsJsonAsync();
    }

    private async Task<ModelResponse?> UpdateModelsJsonAsync()
    {
        string response = string.Empty;
        using (var httpClient = new HttpClient())
            response = await httpClient.GetStringAsync("https://openrouter.ai/api/v1/models");

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var Models = JsonSerializer.Deserialize<ModelResponse>(response, options);

        var baseDir = AppContext.BaseDirectory;
        var basePath = Path.Combine(baseDir, "Storage");

        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        var path = Path.Combine(basePath, "models.json");
        await File.WriteAllTextAsync(path, JsonSerializer.Serialize(Models, options));

        Console.WriteLine("- Lista de Modelos LLM Atualizada!");
        return Models;
    }

    public ModelResponse GetModels() => Models!;
}
