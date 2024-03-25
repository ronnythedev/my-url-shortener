FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/url-shortener-api/url-shortener-api.csproj", "src/url-shortener-api/"]
RUN dotnet restore "src/url-shortener-api/url-shortener-api.csproj"
COPY . .
WORKDIR "/src/src/url-shortener-api"
RUN dotnet build "url-shortener-api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "url-shortener-api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "url-shortener-api.dll"]
