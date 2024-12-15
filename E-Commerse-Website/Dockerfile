# Use the official .NET 8 runtime as the base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET 8 SDK as the base image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["E_Commerse_Website/E_Commerse_Website.csproj", "E_Commerse_Website/"]
RUN dotnet restore "E_Commerse_Website/E_Commerse_Website.csproj"

# Copy the entire source code and build the app
COPY . .
WORKDIR "/src/E_Commerse_Website"
RUN dotnet build "E_Commerse_Website.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "E_Commerse_Website.csproj" -c Release -o /app/publish

# Use the runtime image to host the published app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "E_Commerse_Website.dll"]
