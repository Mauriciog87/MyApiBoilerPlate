# Multi-stage Dockerfile for MyApiBoilerPlate
# Following docker-expert best practices

# Stage 1: Base Runtime
# Using aspnet runtime for a smaller final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Security: Create a non-root user
# Alpine uses 'addgroup' and 'adduser'
RUN addgroup -S appgroup && adduser -S appuser -G appgroup
USER appuser

# Stage 2: Build
# Using SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Optimization: Copy and restore dependencies first to leverage layer caching
COPY ["src/MyApiBoilerPlate.API/MyApiBoilerPlate.API.csproj", "src/MyApiBoilerPlate.API/"]
COPY ["src/MyApiBoilerPlate.Application/MyApiBoilerPlate.Application.csproj", "src/MyApiBoilerPlate.Application/"]
COPY ["src/MyApiBoilerPlate.Domain/MyApiBoilerPlate.Domain.csproj", "src/MyApiBoilerPlate.Domain/"]
COPY ["src/MyApiBoilerPlate.Infrastructure/MyApiBoilerPlate.Infrastructure.csproj", "src/MyApiBoilerPlate.Infrastructure/"]

RUN dotnet restore "src/MyApiBoilerPlate.API/MyApiBoilerPlate.API.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/src/MyApiBoilerPlate.API"
RUN dotnet build "MyApiBoilerPlate.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MyApiBoilerPlate.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Healthcheck to monitor app status
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "MyApiBoilerPlate.API.dll"]
