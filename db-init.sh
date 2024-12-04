#!/bin/bash

echo "Waiting for the database to be ready..."
until pg_isready -h arcadia_postgresql -p 5432 -U arcadia; do
    sleep 2
done

if [ "$RESET_DB" = "true" ]; then
    echo "Resetting the database..."
    dotnet ef database drop --force --context Arcadia.API.Data.ApplicationDbContext --project /source/Arcadia.API &&
    dotnet ef migrations remove --force --context Arcadia.API.Data.ApplicationDbContext --project /source/Arcadia.API &&
    rm -rf /source/Arcadia.API/Migrations &&
    dotnet ef migrations add InitialIdentitySchema --context Arcadia.API.Data.ApplicationDbContext --output-dir Migrations --project /source/Arcadia.API &&
    dotnet ef database update --context Arcadia.API.Data.ApplicationDbContext --project /source/Arcadia.API
else
    echo "Running migrations..."
    dotnet ef migrations add InitialIdentitySchema --context Arcadia.API.Data.ApplicationDbContext --output-dir Migrations --project /source/Arcadia.API &&
    dotnet ef database update --context Arcadia.API.Data.ApplicationDbContext --project /source/Arcadia.API
fi

echo "Done."
