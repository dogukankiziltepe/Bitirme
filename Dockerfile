# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files
COPY . ./

# Restore dependencies
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o /out

# Use the official .NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the built application from the build stage
COPY --from=build /out .

# Expose the port the application runs on
EXPOSE 8080

# Set the entry point for the application
ENTRYPOINT ["dotnet", "Bitirme.dll"]