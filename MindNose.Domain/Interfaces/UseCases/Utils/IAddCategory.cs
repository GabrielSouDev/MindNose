using MindNose.Domain.Nodes;
using MindNose.Domain.Results;

namespace MindNose.Domain.Interfaces.UseCases.Utils;

public interface IAddCategory
{
    Task<CategoryResult> ExecuteAsync(string category);
}