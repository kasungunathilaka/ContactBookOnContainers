#FROM microsoft/aspnetcore:2.0 AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM microsoft/aspnetcore-build:2.0 AS build
#WORKDIR /src
#COPY *.sln ./
#COPY WebSPA/WebSPA.csproj WebSPA/
#RUN dotnet restore
#COPY . .
#WORKDIR /src/WebSPA
#RUN dotnet build -c Release -o /app
#
#FROM build AS publish
#RUN dotnet publish -c Release -o /app
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app .
#ENTRYPOINT ["dotnet", "WebSPA.dll"]
#

FROM node:8.9-alpine as builder

# --------------------------------------
# Install npm packages
# --------------------------------------

WORKDIR /app
RUN npm i -g @angular/cli
COPY WebSPA/package.json ./package.json
RUN npm install 

COPY WebSPA .

# --------------------------------------
# Build PROD & BETA
# --------------------------------------
RUN ng build --prod --build-optimizer

# --------------------------------------
# Create final image
# --------------------------------------
FROM nginx:1.13.1

WORKDIR /app
COPY --from=builder /app/dist .

RUN  rm -rf /usr/share/nginx/html/* && \
	 cp -R /app/src/* /usr/share/nginx/html/  

COPY WebSPA/Docker.nginx.default.conf /etc/nginx/conf.d/default.conf