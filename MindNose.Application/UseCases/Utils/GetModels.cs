using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Interfaces.UseCases.Utils;
using MindNose.Domain.LLMModels;


namespace MindNose.Application.UseCases.Utils;

public class GetModels : IGetModels
{
    private readonly IModelsStorageService _modelsStorageService;

    public GetModels (IModelsStorageService modelsStorageService)
    {
        _modelsStorageService = modelsStorageService;
    }
    public ModelResponse Execute() =>
        _modelsStorageService.GetModels();
}