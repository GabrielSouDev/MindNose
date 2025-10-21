using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Interfaces.UseCases.Utils;
using MindNose.Domain.LLMModels;
using MindNose.Domain.Nodes;
using MindNose.Domain.Results;
using MindNose.Domain.Services;

namespace MindNose.Application.UseCases.Utils;

public class GetCategoryList : IGetCategoryList
{
    private readonly ICategoryService _categoryService;

    public GetCategoryList(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public List<CategoryResult> ExecuteAsync()
    {
        return  _categoryService.GetCategories();
    }
}
