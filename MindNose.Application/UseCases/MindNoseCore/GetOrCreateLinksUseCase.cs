using MindNose.Application.Services;
using MindNose.Domain.Exceptions;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Interfaces.UseCases.MindNoseCore;
using MindNose.Domain.Nodes;
using MindNose.Domain.Request;
using MindNose.Domain.Results;

namespace MindNose.Application.UseCases.MindNoseCore;

public class GetOrCreateLinksUseCase : IGetOrCreateLinksUseCase
{
    private readonly IOpenRouterService _openRouterService;
    private readonly IEmbeddingService _embeddingService;
    private readonly ICategoryService _categoryService;
    private readonly INeo4jService _neo4jService;

    public GetOrCreateLinksUseCase(IOpenRouterService openRouterService, IEmbeddingService embeddingService, ICategoryService categoryService, INeo4jService neo4jService)
    {
        _openRouterService = openRouterService;
        _embeddingService = embeddingService;
        _categoryService = categoryService;
        _neo4jService = neo4jService;
    }

    public async Task<Links> ExecuteAsync(LinksRequest request)
    {
        if(!_categoryService.ContainsCategory(request.Category))
            throw new CategoryNotFoundException(); 

        var categoryResult = _categoryService.GetCategory(request.Category);
        request.SetCategorySummary(categoryResult.Summary);

        var link = new Links();
        var notEmbeddingedTermResult = new LinksResult();
        
        try
        {
            link = await _neo4jService.IfExistsReturnLinksAsync(request);
        }
        catch (LinksNotFoundException)
        {
            notEmbeddingedTermResult = await _openRouterService.CreateTermResultAsync(request);

            LinksResult termResult = await _embeddingService.MakeEmbeddingAsync(notEmbeddingedTermResult);
            termResult.WasCreated = true;
            link = await _neo4jService.SaveTermResultAndReturnLinksAsync(termResult);
        }

        if (link is not null)
            return link;

        throw new LinksNotCreatedException();
    }
}
