using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.LLMModels;

namespace MindNose.Apresentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public partial class UtilsController : ControllerBase
{
    private readonly IModelsStorageService _storageService;

    public UtilsController(IModelsStorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpGet("GetModelsList")]
    public ModelResponse GetModelsIdList() =>
        _storageService.GetModels();
}