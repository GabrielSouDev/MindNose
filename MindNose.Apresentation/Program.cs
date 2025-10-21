using MindNose.Apresentation.Extensions;
using MindNose.Apresentation.Extensions.Services;
using MindNose.Apresentation.Extensions.UseCases;
using MindNose.Domain.Consts;

var embeddingModel = EmbeddingModel.E5_Base;
bool isLocalEmbedding = false;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8080");

builder.AddConfigurations();

builder.AddEmbeddingClient(isLocalEmbedding, embeddingModel);
builder.AddClients();

builder.AddServices();

builder.AddUtilsUseCase();
builder.AdMindNoseCoreUseCase();

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

await app.InitializeAsync(isLocalEmbedding);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.MapControllers();

app.Run();