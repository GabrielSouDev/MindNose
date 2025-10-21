using MindNose.Domain.Nodes;
using MindNose.Domain.Request;
using MindNose.Domain.Results;

namespace MindNose.Domain.Interfaces.Services;

public interface IEmbeddingService
{
    Task<LinksResult> MakeEmbeddingAsync(LinksResult termResult);
}