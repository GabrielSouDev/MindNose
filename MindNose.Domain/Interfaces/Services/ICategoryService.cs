using MindNose.Domain.Nodes;
using MindNose.Domain.Results;

namespace MindNose.Domain.Interfaces.Services;

public interface ICategoryService
{
    bool ContainsCategory(string category);
    CategoryResult GetCategory(string category);
    void AddCategory(CategoryResult categories);
    List<CategoryResult> GetCategories();
    void SetCategories(List<CategoryResult> categories);
}