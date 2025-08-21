using RelationalGraph.Application.Interfaces.Services;
using RelationalGraph.Application.Operations;
using RelationalGraph.Domain.Exceptions;
using RelationalGraph.Domain.Nodes;

namespace RelationalGraph.Application.Services;

public class RelationalGraphService : IRelationalGraphService
{
    private readonly IOpenRouterService _openRouterService;
    private readonly INeo4jService _neo4jService;

    public RelationalGraphService(IOpenRouterService openRouterService, INeo4jService neo4jService)
    {
        _openRouterService = openRouterService;
        _neo4jService = neo4jService;
    }
  
    public async Task<Links> CreateOrReturnLinks(string category, string term)
    {
        category = category.ToPascalCase();
        term = term.ToPascalCase();

        Links? link = await _neo4jService.IfNodeExistsReturnLinks(category, term);

        if (link is not null)
            return link;

        var termObj = await _openRouterService.CreateTermResult(category, term);

        link = await _neo4jService.SaveTermResultAndReturnIntoLinks(termObj); //cria nó no neo4j
        if (link is not null)
            return link;

        throw new LinkNotFoundOrCreatedException();
    }
}
