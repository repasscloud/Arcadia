#!/bin/zsh

# Script Name: concatenate_csv.sh
# Description: Concatenates all CSV files in a specified directory into a single CSV file,
#              ensuring that the header row is included only once.
# Usage: ./concatenate_csv.sh

# Set the directory containing the CSV files
INPUT_DIR="Data/CorporateTravelAssistant/NewBooking_Intents"

# Set the output file path
OUTPUT_FILE="$INPUT_DIR/NewBooking_Intents.csv"

# Check if the input directory exists
if [[ ! -d "$INPUT_DIR" ]]; then
  echo "Error: Directory '$INPUT_DIR' does not exist."
  exit 1
fi

# Find all CSV files in the input directory
CSV_FILES=("$INPUT_DIR"/*.csv)

# Check if there are any CSV files to process
if [[ ${#CSV_FILES[@]} -eq 0 ]]; then
  echo "Error: No CSV files found in directory '$INPUT_DIR'."
  exit 1
fi

# Initialize a flag to handle headers
HEADER_INCLUDED=false

# Remove the output file if it already exists to prevent appending to old data
if [[ -f "$OUTPUT_FILE" ]]; then
  rm "$OUTPUT_FILE"
fi

# Iterate over each CSV file
for csv in "${CSV_FILES[@]}"; do
  if [[ "$csv" == "$OUTPUT_FILE" ]]; then
    # Skip the output file if it somehow exists in the input directory
    continue
  fi

  if [[ "$HEADER_INCLUDED" = false ]]; then
    # For the first CSV file, include the header
    cat "$csv" >> "$OUTPUT_FILE"
    HEADER_INCLUDED=true
    echo "Added header from '$csv'."
  else
    # For subsequent CSV files, skip the header (assuming the first line is the header)
    tail -n +2 "$csv" >> "$OUTPUT_FILE"
    echo "Appended data from '$csv' without header."
  fi
done

echo "All CSV files have been concatenated into '$OUTPUT_FILE'."
