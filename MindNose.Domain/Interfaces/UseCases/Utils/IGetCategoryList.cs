using MindNose.Domain.LLMModels;
using MindNose.Domain.Nodes;
using MindNose.Domain.Results;

namespace MindNose.Domain.Interfaces.UseCases.Utils;

public interface IGetCategoryList
{
    List<CategoryResult> ExecuteAsync();
}