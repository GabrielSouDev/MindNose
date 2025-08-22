using MindNose.Domain.LLMModels;

namespace MindNose.Domain.Interfaces.Services
{
    public interface IModelsStorageService
    {
        Task<ModelResponse?> UpdateModelsJson();
        Task<ModelResponse?> LoadModelsJson();
    }
}