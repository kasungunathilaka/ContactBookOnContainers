FROM microsoft/aspnetcore:latest AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:latest AS build
WORKDIR /src
COPY *.sln ./
COPY src/Services/ContactService/ContactService.API/ContactService.API.csproj ContactService.API/
COPY src/BuildingBlocks/WebHost/WebHost.Customization/WebHost.Customization.csproj src/BuildingBlocks/WebHost/WebHost.Customization/
RUN dotnet restore
COPY . .
WORKDIR /src/src/Services/ContactService/ContactService.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

#WORKDIR /app
#COPY /ContactService.API/entrypoint.sh .
#RUN chmod +x entrypoint.sh
#CMD /bin/bash entrypoint.sh

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY src/Services/ContactService/ContactService.API/entrypoint.sh .
#RUN chmod +x entrypoint.sh
#CMD /bin/bash entrypoint.sh
ENTRYPOINT ["dotnet", "ContactService.API.dll"]
