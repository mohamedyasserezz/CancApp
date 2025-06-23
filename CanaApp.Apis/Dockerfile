# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files first for better layer caching
COPY ["CanaApp.Apis/CanaApp.Apis.csproj", "CanaApp.Apis/"]
COPY ["CanaApp.Application/CanaApp.Application.csproj", "CanaApp.Application/"]
COPY ["CanaApp.Domain/CanaApp.Domain.csproj", "CanaApp.Domain/"]
COPY ["CanaApp.Persistance/CanaApp.Persistance.csproj", "CanaApp.Persistance/"]
COPY ["CancApp.Shared/CancApp.Shared.csproj", "CancApp.Shared/"]

# Restore dependencies
RUN dotnet restore "./CanaApp.Apis/CanaApp.Apis.csproj"

# Copy source code
COPY . .

# Build the application
WORKDIR "/src/CanaApp.Apis"
RUN dotnet build "./CanaApp.Apis.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CanaApp.Apis.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app

# Copy published application
COPY --from=publish /app/publish .

# Create logs directory
RUN mkdir -p /app/logs && chown $APP_UID:$APP_UID /app/logs

# Set environment variables for better performance
ENV ASPNETCORE_URLS=http://+:8080;https://+:8081
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "CanaApp.Apis.dll"]