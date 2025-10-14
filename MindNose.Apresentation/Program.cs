using Microsoft.Extensions.Logging;
using MindNose.Application.UseCases;
using MindNose.Domain.Configurations;
using MindNose.Domain.Consts;
using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.Interfaces.Commons;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Interfaces.UseCases;
using MindNose.Domain.Services;
using MindNose.Infrastructure.Embedding;
using MindNose.Infrastructure.HttpClients;
using MindNose.Infrastructure.Persistence;

var embeddingModel = EmbeddingModel.E5_Base;
bool isLocalEmbedding = false;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8080");

builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<OpenRouterSettings>(builder.Configuration.GetSection("OpenRouterSettings"));
builder.Services.Configure<Neo4jSettings>(neo4j =>
{
    neo4j.Host = builder.Configuration["DBHOST"] ?? 
        builder.Configuration["Neo4jSettings:Host"] ?? 
        throw new Exception("Não foi possivel localizar o host do Neo4j.");

    neo4j.Username = builder.Configuration["DBUSER"] ?? 
        builder.Configuration["Neo4jSettings:Username"] ?? 
        throw new Exception("Não foi possivel localizar o username do Neo4j.");

    neo4j.Password = builder.Configuration["DBPASSWORD"] ?? 
        builder.Configuration["Neo4jSettings:Password"] ?? 
        throw new Exception("Não foi possivel localizar o password do Neo4j.");
});

builder.Services.AddSingleton<INeo4jClient, Neo4jClient>();
builder.Services.AddScoped<IOpenRouterClient, OpenRouterClient>();

if(isLocalEmbedding)
{
    builder.Services.AddSingleton<IEmbeddingClient>(e => new LocalEmbeddingClient(embeddingModel));
} 
else
{
    builder.Services.AddSingleton<IEmbeddingClient>(sp=> new OpenAIEmbeddingClient(builder.Configuration["OpenAI:ApiKey"]!));
}

builder.Services.AddScoped<IEmbeddingService, EmbedingService>();
builder.Services.AddSingleton<IModelsStorageService, ModelsStorageService>();
builder.Services.AddScoped<INeo4jService, Neo4jService>();
builder.Services.AddScoped<IOpenRouterService, OpenRouterService>();

builder.Services.AddScoped<IGetLinks, GetLinks>();
builder.Services.AddScoped<ICreateOrGetLinksUseCase, CreateOrGetLinksUseCase>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors( options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var modelsStorageService = scope.ServiceProvider.GetRequiredService<IModelsStorageService>();
    var neo4jClient = scope.ServiceProvider.GetRequiredService<INeo4jClient>();

    if(isLocalEmbedding)
    {
        var embeddingClient = scope.ServiceProvider.GetRequiredService<IEmbeddingClient>();
        if(embeddingClient is LocalEmbeddingClient _embeddingClient)
            await _embeddingClient.InitializeAsync();
    }

    await modelsStorageService.InitializeAsync();

    if(neo4jClient is Neo4jClient _neo4jclient)
        await _neo4jclient.InitializeAsync();

    Console.WriteLine();
    Console.WriteLine("***** ------ MindNose Iniciado ------ *****");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.MapControllers();

app.Run();