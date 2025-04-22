using Microsoft.OpenApi.Writers;
using RelationalGraph.Application.Interfaces.Clients;
using RelationalGraph.Application.Interfaces.Services;
using RelationalGraph.Application.Services;
using RelationalGraph.Domain.Configuration;
using RelationalGraph.Infrastructure.HttpClients;
using RelationalGraph.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<OpenRouterSettings>(builder.Configuration.GetSection("OpenRouter"));
builder.Services.Configure<Neo4jSettings>(builder.Configuration.GetSection("Neo4j"));

builder.Services.AddSingleton<ModelsStorageService>();

builder.Services.AddSingleton<INeo4jClient, Neo4jClient>();
builder.Services.AddHttpClient<IOpenRouterClient, OpenRouterClient>();

builder.Services.AddScoped<INeo4jService, Neo4jService>();
builder.Services.AddScoped<IOpenRouterService, OpenRouterService>();

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
