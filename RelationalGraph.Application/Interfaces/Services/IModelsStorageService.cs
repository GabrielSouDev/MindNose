using RelationalGraph.Domain.LLMModels;

namespace RelationalGraph.Application.Interfaces.Services
{
    public interface IModelsStorageService
    {
        Task UpdateModelsJson();
        Task<ModelResponse> LoadModelsJson();
    }
}