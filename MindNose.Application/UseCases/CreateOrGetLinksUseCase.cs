using MethodTimer;
using MindNose.Domain.Exceptions;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Interfaces.UseCases;
using MindNose.Domain.Nodes;
using MindNose.Domain.Request;
using MindNose.Domain.Results;
using System.Diagnostics;

namespace MindNose.Application.UseCases;

public class CreateOrGetLinksUseCase : ICreateOrGetLinksUseCase
{
    private readonly IOpenRouterService _openRouterService;
    private readonly IEmbeddingService _embeddingService;
    private readonly INeo4jService _neo4jService;

    public CreateOrGetLinksUseCase(IOpenRouterService openRouterService, IEmbeddingService embeddingService, INeo4jService neo4jService)
    {
        _openRouterService = openRouterService;
        _embeddingService = embeddingService;
        _neo4jService = neo4jService;
    }

    public async Task<Links> ExecuteAsync(LinksRequest request)
    {
        var link = new Links();
        var notEmbeddingedTermResult = new TermResult();
        
        try
        {
            link = await _neo4jService.IfExistsReturnLinksAsync(request);
        }
        catch (LinksNotFoundException)
        {
            notEmbeddingedTermResult = await _openRouterService.CreateTermResultAsync(request);

            TermResult termResult = _embeddingService.MakeEmbeddingAsync(notEmbeddingedTermResult);

            link = await _neo4jService.SaveTermResultAndReturnLinksAsync(termResult);
        }

        if (link is not null)
            return link;

        throw new LinksNotCreatedException();
    }
}
