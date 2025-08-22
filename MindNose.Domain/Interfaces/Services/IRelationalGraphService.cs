using MindNose.Domain.Nodes;

namespace MindNose.Domain.Interfaces.Services;

public interface IRelationalGraphService
{
    Task<Links> CreateOrReturnLinks(string category, string term);
}