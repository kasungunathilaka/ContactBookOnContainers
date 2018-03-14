FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY ContactService.API/ContactService.API.csproj ContactService.API/
RUN dotnet restore
COPY . .
WORKDIR /src/ContactService.API
RUN dotnet build -c Release -o /app

RUN chmod +x entrypoint.sh
CMD /bin/bash entrypoint.sh

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ContactService.API.dll"]
