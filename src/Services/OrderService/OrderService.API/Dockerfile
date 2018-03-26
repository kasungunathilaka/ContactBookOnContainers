FROM microsoft/aspnetcore:latest AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:latest AS build
WORKDIR /src
COPY *.sln ./
COPY src/Services/OrderService/OrderService.API/OrderService.API.csproj src/Services/OrderService/OrderService.API/
COPY src/BuildingBlocks/WebHost/WebHost.Customization/WebHost.Customization.csproj src/BuildingBlocks/WebHost/WebHost.Customization/
COPY src/BuildingBlocks/HealthChecks/src/Microsoft.AspNetCore.HealthChecks/Microsoft.AspNetCore.HealthChecks.csproj src/BuildingBlocks/HealthChecks/src/Microsoft.AspNetCore.HealthChecks/
COPY src/BuildingBlocks/HealthChecks/src/Microsoft.Extensions.HealthChecks/Microsoft.Extensions.HealthChecks.csproj src/BuildingBlocks/HealthChecks/src/Microsoft.Extensions.HealthChecks/
COPY src/BuildingBlocks/HealthChecks/src/Microsoft.Extensions.HealthChecks.SqlServer/Microsoft.Extensions.HealthChecks.SqlServer.csproj src/BuildingBlocks/HealthChecks/src/Microsoft.Extensions.HealthChecks.SqlServer/
RUN dotnet restore
COPY . .
WORKDIR /src/src/Services/OrderService/OrderService.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OrderService.API.dll"]
