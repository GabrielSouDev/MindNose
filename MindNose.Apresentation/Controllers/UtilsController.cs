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
    [HttpPost("AddCategory")]
    public async Task<IActionResult> AddCategory([FromBody] string category)
    {
        bool sucess;

        try
        {
            sucess = await _addCategory.ExecuteAsync(category);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        if (sucess) return NoContent();

        return Conflict();
    }

    [AllowAnonymous]
    [HttpGet("GetCategories")]
    public List<CategoryResult> GetCategoryList() =>
        _getCategoryList.ExecuteAsync();

    [AllowAnonymous]
    [HttpGet("GetModels")]
    public ModelResponse GetModelsIdList() =>
        _getModels.Execute();
}