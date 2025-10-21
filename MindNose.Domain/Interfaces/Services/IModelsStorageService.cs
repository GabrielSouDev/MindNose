using MindNose.Domain.LLMModels;

namespace MindNose.Domain.Interfaces.Services;

public interface IModelsStorageService
{
    Task InitializeAsync();
    ModelResponse GetModels();
}