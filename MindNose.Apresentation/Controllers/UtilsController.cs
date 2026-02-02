using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindNose.Domain.Consts;
using MindNose.Domain.Interfaces.UseCases.Utils;
using MindNose.Domain.LLMModels;
using MindNose.Domain.Nodes;
using MindNose.Domain.Results;

namespace MindNose.Apresentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public partial class UtilsController : ControllerBase
{
    private readonly IGetModels _getModels;
    private readonly IGetCategoryList _getCategoryList;
    private readonly IAddCategory _addCategory;

    public UtilsController(IGetModels getModels, IGetCategoryList getCategoryList, IAddCategory addCategory)
    {
        _getModels = getModels;
        _getCategoryList = getCategoryList;
        _addCategory = addCategory;
    }

    [Authorize(Roles = Role.Admin)]
    [HttpGet("AddCategory")]
    public async Task<CategoryResult> AddCategory(string category) =>
        await _addCategory.ExecuteAsync(category);

    [AllowAnonymous]
    [HttpGet("GetCategories")]
    public List<CategoryResult> GetCategoryList() =>
        _getCategoryList.ExecuteAsync();

    [AllowAnonymous]
    [HttpGet("GetModels")]
    public ModelResponse GetModelsIdList() =>
        _getModels.Execute();
}