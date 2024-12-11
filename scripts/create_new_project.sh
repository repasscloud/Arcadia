#!/usr/bin/env zsh

# Get the project name from the first argument, or default to "Arcadia"
PROJECT_NAME=${1:-Arcadia}

# Remove existing project files if existing
rm -rf $PROJECT_NAME.Shared
rm -rf $PROJECT_NAME.API
rm -rf $PROJECT_NAME.WebApp
rm -rf $PROJECT_NAME.sln

# Display the project name being used
echo "Using project name: $PROJECT_NAME"

# Create base solution file
dotnet new sln --name $PROJECT_NAME

# Create server render blazor project and add to sln
dotnet new blazor -int Server -ai --no-https --use-program-main --name $PROJECT_NAME.WebApp -f net8.0
dotnet sln add ./$PROJECT_NAME.WebApp/$PROJECT_NAME.WebApp.csproj

# Create shared project and add to sln
dotnet new classlib -f net8.0 --name $PROJECT_NAME.Shared
dotnet sln add ./$PROJECT_NAME.Shared/$PROJECT_NAME.Shared.csproj

# Add shared project reference to WebApp
dotnet add ./$PROJECT_NAME.WebApp/$PROJECT_NAME.WebApp.csproj reference ./$PROJECT_NAME.Shared/$PROJECT_NAME.Shared.csproj

# Create webapiaot project and add to sln
# dotnet new webapiaot -f net8.0 --name $PROJECT_NAME.API --use-program-main
dotnet new webapi -f net8.0 --name $PROJECT_NAME.API --use-program-main --no-https --auth None --use-controllers 
dotnet sln add ./$PROJECT_NAME.API/$PROJECT_NAME.API.csproj

# Add shared project reference to API
dotnet add ./$PROJECT_NAME.API/$PROJECT_NAME.API.csproj reference ./$PROJECT_NAME.Shared/$PROJECT_NAME.Shared.csproj

# Restore all project files
dotnet restore

# Build all projects for testing
dotnet build --no-restore

# Build docker images
docker build --rm --file ./Dockerfile.WebApp -t arcadia.webapp .
