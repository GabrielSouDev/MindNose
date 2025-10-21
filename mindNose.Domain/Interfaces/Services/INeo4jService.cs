using MindNose.Domain.Nodes;
using MindNose.Domain.Request;
using MindNose.Domain.Results;

namespace MindNose.Domain.Interfaces.Services
{
    public interface INeo4jService
    {
        Task<Links?> AddCategory(LinksResult? categoryLinks);
        Task<Links?> GetCategoryList();
        Task<Links?> IfCategoryExistsReturnLinksAsync(string category);
        Task<Links?> IfExistsReturnLinksAsync(LinksRequest request);
        Task<Links?> SaveTermResultAndReturnLinksAsync(LinksResult response);
    }
}