namespace MindNose.Domain.DTO.Cytoscape;
public class CytoscapeDTO
{
    public ElementsDTO Elements { get; set; } = new ElementsDTO();
    public bool WasCreated { get; set; } = false;
}