using RelationalGraph.Application.Interfaces.Clients;
using RelationalGraph.Application.Interfaces.Services;
using RelationalGraph.Application.Services;
using RelationalGraph.Domain.Configuration;
using RelationalGraph.Infrastructure.HttpClients;
using RelationalGraph.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
    options.ListenAnyIP(8081, listenOptions => listenOptions.UseHttps());
});

builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<OpenRouterSettings>(builder.Configuration.GetSection("OpenRouterSettings"));
builder.Services.Configure<Neo4jSettings>(neo4j =>
{
    neo4j.Url = builder.Configuration["DBHOST"] ?? "localhost";
    neo4j.Port = int.TryParse(builder.Configuration["DBPORT"], out var port) ? port : 7687;
    neo4j.Username = builder.Configuration["DBUSER"] ?? "neo4j";
    neo4j.Password = builder.Configuration["DBPASSWORD"] ?? "senha123";
});

builder.Services.AddSingleton<ModelsStorageService>();

builder.Services.AddSingleton<INeo4jClient, Neo4jClient>();
builder.Services.AddHttpClient<IOpenRouterClient, OpenRouterClient>();

builder.Services.AddScoped<INeo4jService, Neo4jService>();
builder.Services.AddScoped<IOpenRouterService, OpenRouterService>();

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
    scope.ServiceProvider.GetRequiredService<ModelsStorageService>();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.MapControllers();

app.Run();