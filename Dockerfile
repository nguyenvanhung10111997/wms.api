# Stage 1: Base Image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Stage 2: Build Image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["wms.api/wwwroot", "wms.api/"]
COPY ["wms.dto/wms.dto.csproj", "wms.dto/"]
COPY ["wms.infrastructure/wms.infrastructure.csproj", "wms.infrastructure/"]
COPY ["wms.business/wms.business.csproj", "wms.business/"]
COPY ["wms.api/wms.api.csproj", "wms.api/"]


# Restore project dependencies
RUN dotnet restore "wms.api/wms.api.csproj"

# Copy the remaining files and build the application
COPY . .
WORKDIR "/src/wms.api"
RUN dotnet build "wms.api.csproj" -c Release -o /app/build

# Stage 3: Publish Image
FROM build AS publish
RUN dotnet publish "wms.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Final Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "wms.api.dll"]