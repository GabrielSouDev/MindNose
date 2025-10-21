using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Interfaces.UseCases.Utils;
using MindNose.Domain.Operations;
using MindNose.Domain.Results;

namespace MindNose.Application.UseCases.Utils;

public class AddCategory : IAddCategory
{
    private readonly INeo4jService _neo4jService;
    private readonly IOpenRouterService _openRouterService;
    private readonly ICategoryService _categoryService;

    public AddCategory(INeo4jService neo4jService, IOpenRouterService openRouterService, ICategoryService categoryService)
    {
        _neo4jService = neo4jService;
        _openRouterService = openRouterService;
        _categoryService = categoryService;
    }

    public async Task<CategoryResult> ExecuteAsync(string category)
    {
        var categoryLinks = await _neo4jService.IfCategoryExistsReturnLinksAsync(category);

        LinksResult linkResult = new();
        if (categoryLinks is null)
        {
            linkResult = await _openRouterService.CreateCategoryResult(category);

            categoryLinks = await _neo4jService.AddCategory(linkResult);
            _categoryService.AddCategory(linkResult.Category);
        }else
            return categoryLinks.Nodes.Select(c =>
                                new CategoryResult()
                                {
                                    Title = c.Properties.Title,
                                    Summary = c.Properties.Summary
                                }).First();

        return linkResult.Category;
    }
}
