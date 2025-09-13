using MindNose.Domain.Nodes;
using MindNose.Domain.Request;

namespace MindNose.Domain.Interfaces.UseCases;

public interface ICreateOrGetLinksUseCase
{
    Task<Links> ExecuteAsync(LinksRequest request);
}