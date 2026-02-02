using MindNose.Domain.Nodes;
using MindNose.Domain.Results;

namespace MindNose.Domain.Interfaces.UseCases.Utils;

public interface IAddCategory
{
    Task<bool> ExecuteAsync(string category);
}