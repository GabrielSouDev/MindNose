using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindNose.Domain.Consts;
using MindNose.Domain.Exceptions;
using MindNose.Domain.Extensions;
using MindNose.Domain.IAChat;
using MindNose.Domain.Interfaces.UseCases.MindNoseCore;
using MindNose.Domain.Request;

namespace MindNose.Apresentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MindNoseCoreController : ControllerBase
{

    private readonly IGetLinks _getLinks;
    private readonly IGetOrCreateLinks _getOrCreateLinks;

    public MindNoseCoreController(IGetLinks getLinks, IGetOrCreateLinks getOrCreateLinksUseCase)
    {
        _getLinks = getLinks;
        _getOrCreateLinks = getOrCreateLinksUseCase;
    }

    [Authorize(Policy = Poly.UserOrAdmin)]
    [HttpGet("GetLink")]
    public async Task<IActionResult> GetLinksAsync([FromQuery] LinksRequest request)
    {
        try
        {
            var link = await _getLinks.ExecuteAsync(request);
            return Ok(link.LinksToDTO());
        }
        catch (LinksNotFoundException)
        {
            return NotFound("Node not found!");
        }
    }

    [Authorize(Policy = Poly.UserOrAdmin)]
    [HttpPost("GetOrCreateLink")]
    public async Task<IActionResult> GetOrCreateLinksAsync([FromBody] LinksRequest request)
    {
        try
        {
            var link = await _getOrCreateLinks.ExecuteAsync(request);

            return Ok(link.LinksToDTO());
        }
        catch (LinksNotCreatedException)
        {
            return NotFound("Node not created!");
        }
        catch (CategoryNotFoundException)
        { 
            return NotFound("Category not found!"); 
        }
    }
}