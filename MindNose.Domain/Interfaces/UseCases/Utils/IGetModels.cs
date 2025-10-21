using MindNose.Domain.LLMModels;

namespace MindNose.Domain.Interfaces.UseCases.Utils;

public interface IGetModels
{
    ModelResponse Execute();
}
