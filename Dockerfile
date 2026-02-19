FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MindNose.Presentation/MindNose.Presentation.csproj", "MindNose.Presentation/"]
COPY ["MindNose.Application/MindNose.Application.csproj", "MindNose.Application/"]
COPY ["MindNose.Domain/MindNose.Domain.csproj", "MindNose.Domain/"]
COPY ["MindNose.Infrastructure/MindNose.Infrastructure.csproj", "MindNose.Infrastructure/"]
RUN dotnet restore "./MindNose.Presentation/MindNose.Presentation.csproj"
COPY . .
WORKDIR "/src/MindNose.Presentation"
RUN dotnet build "./MindNose.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MindNose.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MindNose.Presentation.dll"]
