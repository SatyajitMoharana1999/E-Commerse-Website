# Use the official .NET SDK image for restoring and building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /src

# Copy the csproj file and restore dependencies
COPY ["E-Commerse-Website/E-Commerse-Website.csproj", "E-Commerse-Website/"]
RUN dotnet restore "E-Commerse-Website/E-Commerse-Website.csproj"

# Copy the rest of the app and build it
COPY ["E-Commerse-Website/", "E-Commerse-Website/"]
RUN dotnet build "E-Commerse-Website/E-Commerse-Website.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "E-Commerse-Website/E-Commerse-Website.csproj" -c Release -o /app/publish

# Use the official ASP.NET runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Set the working directory for the final image
WORKDIR /app

# Copy the build from the previous image
COPY --from=build /app/publish .

# Expose the port that the app will run on
EXPOSE 80

# Define the entry point for the container
ENTRYPOINT ["dotnet", "E-Commerse-Website.dll"]
