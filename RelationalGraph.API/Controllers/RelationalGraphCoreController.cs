using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RelationalGraph.Application;
using RelationalGraph.Application.Interfaces.Services;
using RelationalGraph.Application.Operations;

namespace RelationalGraph.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelationalGraphCoreController : ControllerBase
    {
        private readonly IOpenRouterService _openRouterService;
        private readonly INeo4jService _neo4jService;

        public RelationalGraphCoreController(IOpenRouterService openRouterService, INeo4jService neo4jService)
        {
            _openRouterService = openRouterService;
            _neo4jService = neo4jService;
        }

        [HttpGet("Post/{category}/{term}")]
        public async Task<IActionResult> CreateKnowledgeNode(string category, string term)
        {
            //consultar neo4j

            //se existir retornar existente

            //senão faz abaixo
            Prompt prompt = Prompt.CreateKnowledgeNode(category,term);
            string result = await _openRouterService.SearchFirstLevel(prompt);

            //cria nó no neo4j
            return Ok(result);
        }
    }
}
