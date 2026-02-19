using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MindNose.Domain.Configurations;
using MindNose.Domain.Interfaces.Clients;
using MindNose.Infrastructure.HttpClients;
using MindNose.Infrastructure.Persistence;

namespace MindNose.Apresentation.Extensions;

public static class RepositoriesExtensions
{
    public static void AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<UserProfileRepository>();
        builder.Services.AddScoped<ConversationGuideRepository>();
        builder.Services.AddScoped<MessageRepository>();
        builder.Services.AddScoped<ChunkRepository>();
    }
}