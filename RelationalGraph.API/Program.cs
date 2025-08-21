using RelationalGraph.Application.Interfaces.Clients;
using RelationalGraph.Application.Interfaces.Services;
using RelationalGraph.Application.Services;
using RelationalGraph.Domain.Configurations;
using RelationalGraph.Infrastructure.HttpClients;
using RelationalGraph.Infrastructure.Persistence;
using System.Text;

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

builder.Services.AddSingleton<INeo4jClient, Neo4jClient>();
builder.Services.AddScoped<IOpenRouterClient, OpenRouterClient>();

builder.Services.AddScoped<INeo4jService, Neo4jService>();
builder.Services.AddScoped<IOpenRouterService, OpenRouterService>();

builder.Services.AddScoped<IRelationalGraphService, RelationalGraphService>();

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
    scope.ServiceProvider.GetRequiredService<INeo4jClient>();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.MapControllers();

app.Run();