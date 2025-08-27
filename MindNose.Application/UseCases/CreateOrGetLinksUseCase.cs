using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Exceptions;
using MindNose.Domain.Nodes;
using MindNose.Domain.Interfaces.UseCases;
using MindNose.Domain.Request;

namespace MindNose.Application.UseCases;

public class CreateOrGetLinksUseCase : ICreateOrGetLinksUseCase
{
    private readonly IOpenRouterService _openRouterService;
    private readonly INeo4jService _neo4jService;

    public CreateOrGetLinksUseCase(IOpenRouterService openRouterService, INeo4jService neo4jService)
    {
        _openRouterService = openRouterService;
        _neo4jService = neo4jService;
    }
  
    public async Task<Links> ExecuteAsync(LinksRequest request)
    {
        Links? link = await _neo4jService.IfNodeExistsReturnLinks(request);

        if (link is not null)
            return link;

        var termObj = await _openRouterService.CreateTermResult(request);

        link = await _neo4jService.SaveTermResultAndReturnIntoLinks(termObj); //cria nó no neo4j
        if (link is not null)
            return link;

        throw new LinkNotFoundOrCreatedException();
    }
}
