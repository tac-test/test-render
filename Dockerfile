# This stage is used when running from VS in fast mode
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TestRender/TestRender.csproj", "TestRender/"]
RUN dotnet restore "./TestRender/TestRender.csproj"
COPY . .
WORKDIR "/src/TestRender"
RUN dotnet build "./TestRender.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TestRender.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables for Render.com
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# Health check (optional but recommended)
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "TestRender.dll"]