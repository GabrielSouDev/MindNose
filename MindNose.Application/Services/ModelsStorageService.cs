using MindNose.Domain.Interfaces.Commons;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.LLMModels;
using System;
using System.Text.Json;
namespace MindNose.Domain.Services;

public class ModelsStorageService : IModelsStorageService, IInitializable
{
    private ModelResponse? _models = new();
    private readonly string _jsonName;

    public ModelsStorageService()
    {
        var date = DateTime.UtcNow;
        _jsonName = $"{date.Day}-{date.Month}-{date.Year}-LLM-Models.json";
    }

    public async Task InitializeAsync()
    {
        _models = await LoadModelsJsonAsync();
    }
    private async Task<ModelResponse?> LoadModelsJsonAsync()
    {
        var baseDir = AppContext.BaseDirectory;
        var path = Path.Combine(baseDir, "Storage", _jsonName);


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

        var path = Path.Combine(basePath, _jsonName);
        await File.WriteAllTextAsync(path, JsonSerializer.Serialize(Models, options));

        Console.WriteLine("- Lista de Modelos LLM Atualizada!");
        return Models;
    }

    public ModelResponse GetModels() => _models!;
}
