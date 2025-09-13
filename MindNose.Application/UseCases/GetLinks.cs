using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Exceptions;
using MindNose.Domain.Nodes;
using MindNose.Domain.Interfaces.UseCases;
using MindNose.Domain.Request;

namespace MindNose.Application.UseCases;

public class GetLinks : IGetLinks
{
    private readonly IOpenRouterService _openRouterService;
    private readonly INeo4jService _neo4jService;

    public GetLinks(IOpenRouterService openRouterService, INeo4jService neo4jService)
    {
        _openRouterService = openRouterService;
        _neo4jService = neo4jService;
    }
  
    public async Task<Links> ExecuteAsync(LinksRequest request)
    {
        Links? link = await _neo4jService.IfExistsReturnLinksAsync(request);

        if (link is not null)
            return link;

        throw new LinksNotFoundException();
    }
}
