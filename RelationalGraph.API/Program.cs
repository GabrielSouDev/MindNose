using Microsoft.OpenApi.Writers;
using RelationalGraph.Application.Interfaces.Clients;
using RelationalGraph.Application.Interfaces.Services;
using RelationalGraph.Application.Services;
using RelationalGraph.Infrastructure.HttpClients;
using RelationalGraph.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<INeo4jClient, Neo4jClient>();
builder.Services.AddHttpClient<IOpenRouterClient, OpenRouterClient>((httpClient, serviceProvider) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var section = configuration.GetSection("OpenRouter");

    return new OpenRouterClient(httpClient, section);
});
builder.Services.AddScoped<INeo4jService, Neo4jService>();
builder.Services.AddScoped<IOpenRouterService, OpenRouterService>();
builder.Services.AddSingleton<ModelsStorageService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var modelsUpdater = scope.ServiceProvider.GetRequiredService<ModelsStorageService>();
    await modelsUpdater.UpdateModelsJson();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
