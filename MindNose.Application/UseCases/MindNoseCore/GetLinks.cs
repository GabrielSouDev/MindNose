using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Exceptions;
using MindNose.Domain.Nodes;
using MindNose.Domain.Request;
using MindNose.Domain.Interfaces.UseCases.MindNoseCore;

namespace MindNose.Application.UseCases.MindNoseCore;

public class GetLinks : IGetLinks
{
    private readonly INeo4jService _neo4jService;

    public GetLinks(INeo4jService neo4jService)
    {
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
