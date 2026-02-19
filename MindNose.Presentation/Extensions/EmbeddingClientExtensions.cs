using MindNose.Domain.Interfaces.Clients;
using MindNose.Infrastructure.Embedding;
using MindNose.Infrastructure.HttpClients;

namespace MindNose.Apresentation.Extensions;

public static class EmbeddingClientExtensions
{
    public static void AddEmbeddingClient(this WebApplicationBuilder builder, bool isLocalEmbedding, string embeddingModel)
    {
        if (isLocalEmbedding)
        {
            builder.Services.AddSingleton<IEmbeddingClient>(e => new LocalEmbeddingClient(embeddingModel));
        }
        else
        {
            builder.Services.AddSingleton<IEmbeddingClient>(sp => 
                new OpenAIEmbeddingClient(builder.Configuration["OpenAIApiKey"] ?? 
                                          builder.Configuration["OpenAI:ApiKey"] ?? 
                                          throw new Exception("Não foi possivel localizar Configurar o OpenAIClient!")));
        }
    }
}