#!/bin/bash

echo "Waiting for the database to be ready..."
until pg_isready -h db -p 5432 -U admin; do
    sleep 2
done

if [ "$RESET_DB" = "true" ]; then
    echo "Resetting the database..."
    dotnet ef database drop --force --context Arcadia.API.Data.ApplicationDbContext &&
    dotnet ef migrations remove --force --context Arcadia.API.Data.ApplicationDbContext &&
    dotnet ef migrations add InitDb --context Arcadia.API.Data.ApplicationDbContext --output-dir Migrations &&
    dotnet ef database update --context Arcadia.API.Data.ApplicationDbContext
else
    echo "Running migrations..."
    dotnet ef migrations add InitDb --context Arcadia.API.Data.ApplicationDbContext --output-dir Migrations &&
    dotnet ef database update --context Arcadia.API.Data.ApplicationDbContext
fi

echo "Done."
