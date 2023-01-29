FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SmartCarWashTest.Sale.WebApi/SmartCarWashTest.Sale.WebApi.csproj", "SmartCarWashTest.Sale.WebApi/"]
COPY ["SmartCarWashTest.Common.Converters.csproj", "SmartCarWashTest.Common.Converters/"]
COPY ["SmartCarWashTest.EventBus.csproj", "SmartCarWashTest.EventBus/"]
COPY ["SmartCarWashTest.EventBusRabbitMq.csproj", "SmartCarWashTest.EventBusRabbitMq/"]
COPY ["SmartCarWashTest.Logger/SmartCarWashTest.Logger.csproj", "SmartCarWashTest.Logger/"]
COPY ["SmartCarWashTest.Sale.WebApi.DTOs/SmartCarWashTest.Sale.WebApi.DTOs.csproj", "SmartCarWashTest.Sale.WebApi.DTOs/"]
RUN dotnet restore "SmartCarWashTest.Sale.WebApi/SmartCarWashTest.Sale.WebApi.csproj"
COPY . .
WORKDIR "/src/SmartCarWashTest.Sale.WebApi"
RUN dotnet build "SmartCarWashTest.Sale.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartCarWashTest.Sale.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartCarWashTest.Sale.WebApi.dll"]
