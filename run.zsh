#!/usr/bin/env zsh

# Function to display the menu
show_menu() {
  echo "Select an option:"
  echo "1) Option 1"
  echo "2) Option 2"
  echo "3) Option 3"
  echo "4) Exit"
  read -p "Enter your choice: " choice
  echo $choice
}

# Check if an argument is provided
if [ $# -eq 1 ]; then
  input="$1"
else
  # No argument provided; display the menu
  input=$(show_menu)
fi

# Switch case based on input
case $input in
  1 | "Option 1")
    echo "You selected Option 1"
    # Fill in logic for Option 1 here
    ;;
  2 | "Option 2")
    echo "You selected Option 2"
    # Fill in logic for Option 2 here
    ;;
  3 | "Option 3")
    echo "You selected Option 3"
    # Fill in logic for Option 3 here
    ;;
  4 | "Exit")
    echo "Exiting..."
    exit 0
    ;;
  *)
    echo "Error: Invalid option provided!"
    exit 1
    ;;
esac


# recreate the pfx file
# recreate the docker volumes
# remove all images
