#!/usr/bin/env bash

# Function to update DB_ACTION in the .env file
update_db_action() {
  local env_file=$1
  local new_action=$2

  # Validate new_action input
  if [[ "$new_action" != "INIT" && "$new_action" != "RESET" && "$new_action" != "NONE" ]]; then
    echo "Error: Invalid action. Use INIT, RESET, or NONE."
    return 1
  fi

  # Check if the .env file exists
  if [[ ! -f "$env_file" ]]; then
    echo "Error: .env file not found at $env_file"
    return 1
  fi

  # Use sed to replace the DB_ACTION line with the new value
  sed -i -E "s/^DB_ACTION=.*/DB_ACTION=$new_action/" "$env_file"

  # Confirm the change
  echo "DB_ACTION updated to $new_action in $env_file"
}

# Function to confirm user input
confirm() {
  read -p "Are you sure? [y/n]: " confirm_choice
  case $confirm_choice in
    [Yy]*) return 0 ;;  # Proceed if the user enters Y or y
    *) return 1 ;;      # Bypass for any other input
  esac
}

# Function to display the menu
show_menu() {
  echo "1) Start Arcadia"
  echo "2) Reset certificates (replace PFX files)"
  echo "3) Init Arcadia setup (fresh install) and start"
  echo "4) Reset Arcadia DB and start"
  echo "5) Update DB schema and start"
  echo "6) Stop Arcadia (compose down)"
  echo "7) Reset certificates and stop Arcadia (no restart)"
  echo "8) Reset Arcadia DB, reset certificates, and start"
  echo "9) Exit"
  read -p "Enter your choice: " choice
}

# Check if an argument is provided
if [ $# -eq 1 ]; then
  input="$1"
else
  # No argument provided; display the menu
  show_menu
  input="$choice"
fi

# Switch case based on input
case $input in
  1 | "Start Arcadia")
    echo "Start Arcadia"
    # Brings up the entire Docker Compose environment (Blazor, WebAPI, SQL, and DB-INIT projects) with the current configuration.
    update_db_action "./.env" "NONE"
    docker compose up -d
    ;;

  2 | "Reset certificates")
    echo "Reset PFX certificates"
    # Fill in logic for Option 2 here
    echo "To reset PFX certificates, containers will be stopped and rebuilt, they need to be started again (option 1)."
    if confirm; then
      docker compose down 2>/dev/null
      /bin/sh Scripts/setup_certificates_and_volumes.sh
      docker compose build
    else
      echo "Cancelling reset of PFX certificates."
    fi
    ;;

  3 | "Init Arcadia setup")
    echo "Init Arcadia setup (fresh install) and start"
    # Performs a full reset of the environment, initializing a fresh database (wiped and rebuilt from scratch) and starting all services.
    docker compose down 2>/dev/null
    ./Scripts/setup_certificates_and_volumes.sh
    update_db_action "./.env" "INIT"
    docker compose up -d --build
    ;;

  4 | "Reset DB")
    echo "Reset Arcadia DB and start"
    # Wipes the database and recreates it from scratch (similar to a fresh install), but does not perform any other resets (like certificates).
    docker compose down 2>/dev/null
    update_db_action "./.env" "INIT"
    docker compose up -d --build
    ;;
  
  5 | "Update schema")
    echo "Update DB schema and start"
    # Applies schema updates or migrations to the database without wiping it and starts the system.
    docker compose down 2>/dev/null
    update_db_action "./.env" "RESET"
    docker compose up -d --build
    ;;

  6 | "Stop Arcadia")
    echo "Stop Arcadia"
    # Brings down the entire Docker Compose setup (all services stopped and containers removed, but volumes are preserved).
    docker compose down 2>/dev/null
    ;;

  7 | "Reset certificates")
    echo "Reset certificates and stop Arcadia (no restart)"
    # Stops the system and allows the user to replace PFX certificates in the attached volumes. Does not restart the system.
    docker compose down 2>/dev/null
    ./Scripts/setup_certificates_and_volumes.sh
    ;;
  
  8 | "Reset Arcadia to fresh")
    echo "Init Arcadia setup (fresh install) and start"
    # P Performs a combination of resetting the database and resetting the certificates, then starts the system.
    docker compose down 2>/dev/null
    ./Scripts/setup_certificates_and_volumes.sh
    update_db_action "./.env" "INIT"
    docker compose up -d
    ;;

  9 | "Exit")
    echo "Exiting..."
    exit 0
    ;;

  *)
    echo "Error: Invalid option provided!"
    exit 1
    ;;
esac
