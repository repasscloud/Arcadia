#!/bin/bash

# Wait for the PostgreSQL database to be ready
DB_HOST=${DB_HOST:-arcadia_postgresql}
DB_PORT=${DB_PORT:-5432}
DB_USER=${DB_USER:-arcadia}

echo "Waiting for the database to be ready..."
until pg_isready -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER"; do
  sleep 2
done

echo "PostgreSQL is ready. Proceeding with database operations."

case "$DB_ACTION" in
  RESET)
    echo "Resetting the database..."
    dotnet ef database drop --force --context Arcadia.API.Data.ApplicationDbContext --project /source/Arcadia.API &&
    dotnet ef migrations remove --force --context Arcadia.API.Data.ApplicationDbContext --project /source/Arcadia.API &&
    rm -rf /source/Arcadia.API/Migrations &&
    dotnet ef migrations add InitialIdentitySchema --context Arcadia.API.Data.ApplicationDbContext --output-dir Migrations --project /source/Arcadia.API &&
    dotnet ef database update --context Arcadia.API.Data.ApplicationDbContext --project /source/Arcadia.API
    ;;
  INIT)
    echo "Initializing the database..."
    if [ ! -d "/source/Arcadia.API/Migrations" ] || [ -z "$(ls -A /source/Arcadia.API/Migrations)" ]; then
      echo "No migrations found. Creating initial migration..."
      dotnet ef migrations add InitialIdentitySchema --context Arcadia.API.Data.ApplicationDbContext --output-dir Migrations --project /source/Arcadia.API
    fi
    dotnet ef database update --context Arcadia.API.Data.ApplicationDbContext --project /source/Arcadia.API
    ;;
  *)
    echo "No database action specified. Skipping database operations."
    ;;
esac

echo "Done."

# if [ "$RESET_DB" = "true" ]; then
#     echo "Resetting the database..."
#     dotnet ef database drop --force --context Arcadia.API.Data.ApplicationDbContext --project /source/Arcadia.API &&
#     dotnet ef migrations remove --force --context Arcadia.API.Data.ApplicationDbContext --project /source/Arcadia.API &&
#     rm -rf /source/Arcadia.API/Migrations &&
#     dotnet ef migrations add InitialIdentitySchema --context Arcadia.API.Data.ApplicationDbContext --output-dir Migrations --project /source/Arcadia.API &&
#     dotnet ef database update --context Arcadia.API.Data.ApplicationDbContext --project /source/Arcadia.API
# else
#     echo "Running migrations..."
#     dotnet ef migrations add InitialIdentitySchema --context Arcadia.API.Data.ApplicationDbContext --output-dir Migrations --project /source/Arcadia.API &&
#     dotnet ef database update --context Arcadia.API.Data.ApplicationDbContext --project /source/Arcadia.API
# fi

# echo "Done."
