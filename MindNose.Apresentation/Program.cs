using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Services;
using MindNose.Domain.Configurations;
using MindNose.Infrastructure.HttpClients;
using MindNose.Infrastructure.Persistence;
using MindNose.Application.UseCases;
using MindNose.Domain.Interfaces.UseCases;

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

builder.Services.AddSingleton<ModelsStorageService>();
builder.Services.AddSingleton<IBoltNeo4jClient, BoltNeo4jClient>();

builder.Services.AddScoped<IOpenRouterClient, OpenRouterClient>();
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
    var boltNeo4jClient = scope.ServiceProvider.GetRequiredService<IBoltNeo4jClient>();
    await boltNeo4jClient.InitializeAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.MapControllers();

app.Run();