using MindNose.Application.UseCases.Utils;
using MindNose.Domain.Interfaces.UseCases.Utils;

namespace MindNose.Apresentation.Extensions.UseCases;

public static class UtilsUseCaseExtensions
{
    public static void AddUtilsUseCase(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAddCategory, AddCategory>();
        builder.Services.AddScoped<IGetModels, GetModels>();
        builder.Services.AddScoped<IGetCategoryList, GetCategoryList>();

    }
}
