FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /src
COPY . .

WORKDIR /src/ClientApp
RUN apt-get update && apt-get install -y curl
RUN curl -sL https://deb.nodesource.com/setup_14.x |  bash -
RUN apt-get install -y nodejs
RUN npm install
RUN npm run build

WORKDIR /src
RUN dotnet restore

FROM build AS publish
RUN dotnet publish "Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /src/ClientApp/dist ./ClientApp/dist
ENTRYPOINT ["dotnet", "Server.dll", "--launch-profile", "Docker"]
