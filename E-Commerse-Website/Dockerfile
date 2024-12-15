# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Base image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY ["E-Commerse-Website/E-Commerse-Website.csproj", "E-Commerse-Website/"]
WORKDIR /src/E-Commerse-Website
RUN dotnet restore

# Copy the rest of the app and build it
COPY ["E-Commerse-Website/", "."]
RUN dotnet build -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "E-Commerse-Website.dll"]
