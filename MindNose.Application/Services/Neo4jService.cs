using MindNose.Domain.Exceptions;
using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Nodes;
using MindNose.Domain.Request;
using MindNose.Domain.Results;

namespace MindNose.Application.Services;

public class Neo4jService : INeo4jService
{
    private readonly INeo4jClient _neo4jClient;

    public Neo4jService(INeo4jClient neo4jClient)
    {
        _neo4jClient = neo4jClient;
    }

    public async Task<Links?> SaveTermResultAndReturnLinksAsync(LinksResult TermObject)
    {
        var links = await _neo4jClient.CreateAndReturnLinksAsync(TermObject);

        return links;
    }

    public async Task<Links?> IfExistsReturnLinksAsync(LinksRequest request)
    {
        var result = await _neo4jClient.GetLinksAsync(request);

        return result;
    }

    public async Task<Links?> GetCategoryList()
    {
        Links? categoryLinks = await _neo4jClient.GetCategories();
        if (categoryLinks is not null)
        {
            return categoryLinks;
        }
        throw new LinksNotFoundException();
    }

    public async Task<Links?> IfCategoryExistsReturnLinksAsync(string category)
    {
        var result = await _neo4jClient.GetCategoryNodeAsync(category);
        if (result is not null)
        {
            return result;
        }
        return null;
    }

    public async Task<Links?> AddCategory(LinksResult? categoryLinks)
    {
        var links = await _neo4jClient.CreateCategoryAndReturnLinks(categoryLinks);

        return links;
    }
}