using RelationalGraph.Domain.LLMModels;

namespace RelationalGraph.Application.Interfaces.Services
{
    public interface IModelsStorageService
    {
        Task<ModelResponse?> UpdateModelsJson();
        Task<ModelResponse?> LoadModelsJson();
    }
}