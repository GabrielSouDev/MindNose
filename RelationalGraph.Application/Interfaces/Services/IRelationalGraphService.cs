using RelationalGraph.Domain.Nodes;

namespace RelationalGraph.Application.Interfaces.Services;

public interface IRelationalGraphService
{
    Task<Links> CreateOrReturnLinks(string category, string term);
}