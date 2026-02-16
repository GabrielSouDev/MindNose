using MindNose.Application.Services;
using MindNose.Domain.Exceptions;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Interfaces.UseCases.MindNoseCore;
using MindNose.Domain.Nodes;
using MindNose.Domain.Request;
using MindNose.Domain.Results;

namespace MindNose.Application.UseCases.MindNoseCore;

public class GetOrCreateLinks : IGetOrCreateLinks
{
    private readonly IOpenRouterService _openRouterService;
    private readonly IEmbeddingService _embeddingService;
    private readonly ICategoryService _categoryService;
    private readonly INeo4jService _neo4jService;

    public GetOrCreateLinks(IOpenRouterService openRouterService, IEmbeddingService embeddingService, ICategoryService categoryService, INeo4jService neo4jService)
    {
        _openRouterService = openRouterService;
        _embeddingService = embeddingService;
        _categoryService = categoryService;
        _neo4jService = neo4jService;
    }

    public async Task<Links> ExecuteAsync(LinksRequest request)
    {
        var categoryResult = _categoryService.GetCategory(request.CategoryId);
        if(categoryResult is null)
            throw new CategoryNotFoundException(); 

        request.CategorySummary = categoryResult.GetSummary();

        var link = new Links();
        var notEmbeddedTermResult = new LinksResult();
        
        try
        {
            link = await _neo4jService.IfExistsReturnLinksAsync(request);
        }
        catch (LinksNotFoundException)
        {
            notEmbeddedTermResult = await _openRouterService.CreateTermResultAsync(request);

            LinksResult termResult = await _embeddingService.MakeEmbeddingAsync(notEmbeddedTermResult);
            termResult.WasCreated = true;
            link = await _neo4jService.SaveTermResultAndReturnLinksAsync(termResult);
        }

        if (link is not null)
            return link;

        throw new LinksNotCreatedException();
    }
}
