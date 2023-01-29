FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SmartCarWashTest.CRUD.WebApi/SmartCarWashTest.CRUD.WebApi.csproj", "SmartCarWashTest.CRUD.WebApi/"]
COPY ["SmartCarWashTest.Common.Converters.csproj", "SmartCarWashTest.Common.Converters/"]
COPY ["SmartCarWashTest.EventBus.csproj", "SmartCarWashTest.EventBus/"]
COPY ["SmartCarWashTest.EventBusRabbitMq.csproj", "SmartCarWashTest.EventBusRabbitMq/"]
COPY ["SmartCarWashTest.Common.DataContext.Sqlite/SmartCarWashTest.Common.DataContext.Sqlite.csproj", "SmartCarWashTest.Common.DataContext.Sqlite/"]
COPY ["SmartCarWashTest.Common.EntityModels.Sqlite/SmartCarWashTest.Common.EntityModels.Sqlite.csproj", "SmartCarWashTest.Common.EntityModels.Sqlite/"]
COPY ["SmartCarWashTest.Common.EntityTypeConfigurations.Sqlite/SmartCarWashTest.Common.EntityTypeConfigurations.Sqlite.csproj", "SmartCarWashTest.Common.EntityTypeConfigurations.Sqlite/"]
COPY ["SmartCarWashTest.CRUD.WebApi.Repositories/SmartCarWashTest.CRUD.WebApi.Repositories.csproj", "SmartCarWashTest.CRUD.WebApi.Repositories/"]
COPY ["SmartCarWashTest.CRUD.WebApi.DTOs/SmartCarWashTest.CRUD.WebApi.DTOs.csproj", "SmartCarWashTest.CRUD.WebApi.DTOs/"]
COPY ["SmartCarWashTest.Logger/SmartCarWashTest.Logger.csproj", "SmartCarWashTest.Logger/"]
COPY ["SmartCarWashTest.Common.SeparateMigrations/SmartCarWashTest.Common.SeparateMigrations.csproj", "SmartCarWashTest.Common.SeparateMigrations/"]
RUN dotnet restore "SmartCarWashTest.WebApi/SmartCarWashTest.CRUD.WebApi.csproj"
COPY . .
WORKDIR "/src/SmartCarWashTest.WebApi"
RUN dotnet build "SmartCarWashTest.CRUD.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartCarWashTest.CRUD.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartCarWashTest.CRUD.WebApi.dll"]
