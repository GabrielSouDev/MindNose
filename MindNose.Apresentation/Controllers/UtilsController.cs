using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.LLMModels;

namespace MindNose.Apresentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UtilsController : ControllerBase
{
    private readonly IModelsStorageService _storageService;

    public UtilsController(IModelsStorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpGet("GetModelsList")]
    public IEnumerable<object> GetModelsIdList() =>
        _storageService.GetModels().Data.Select(d => new { Name = d.Name, Id = d.Id }).ToList();
}
