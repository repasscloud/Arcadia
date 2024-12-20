#!/bin/bash

# Check for three arguments
if [ "$#" -ne 3 ]; then
    echo "Usage: $0 <input_file> <output_file> <line_selector>"
    exit 1
fi

# Assign arguments to variables
input_file="$1"
output_file="$2"
selector="$3"
temp_file="$(mktemp)"

# Determine the operating system
os_type="$(uname)"

# Check if the input file exists
if [ ! -f "$input_file" ]; then
    echo "Error: Input file '$input_file' not found."
    exit 1
fi

# Check if the output file exists and try to remove it
if [ -f "$output_file" ]; then
    if ! rm -f "$output_file"; then
        echo "Error: Output file '$output_file' exists and cannot be removed."
        exit 1
    fi
fi

# Step 1: Extract lines matching the selector and the second field after the tab
grep "^${selector}" "$input_file" | cut -f2 > "$temp_file"

# OS-specific operations
if [ "$os_type" = "Darwin" ]; then
    # macOS-specific commands

    # Step 2: Replace all instances of ’ with '
    sed -i '' "s/’/'/g" "$temp_file"

    # Step 3: Remove all instances of the " character
    sed -i '' 's/"//g' "$temp_file"

    # Step 4: Remove leading patterns like 'word:' and ': '
    sed -i '' -E 's/^[[:alnum:]]+:[[:space:]]+//' "$temp_file"
    sed -i '' -E 's/^:[[:space:]]+//' "$temp_file"

elif [ "$os_type" = "Linux" ]; then
    # Linux-specific commands

    # Step 2: Replace all instances of ’ with '
    sed -i "s/’/'/g" "$temp_file"

    # Step 3: Remove all instances of the " character
    sed -i 's/"//g' "$temp_file"

    # Step 4: Remove leading patterns like 'word:' and ': '
    sed -i -E 's/^[[:alnum:]]+:[[:space:]]+//' "$temp_file"
    sed -i -E 's/^:[[:space:]]+//' "$temp_file"

else
    echo "Unsupported operating system: $os_type"
    rm -f "$temp_file"
    exit 1
fi

# Step 5: Remove lines with only a single word
grep -E -v '^[[:alnum:]]+$' "$temp_file" > "${temp_file}_filtered" && mv "${temp_file}_filtered" "$temp_file"

# Step 6: Remove lines with only a single character
grep -E -v '^[[:space:]]*.$' "$temp_file" > "${temp_file}_filtered" && mv "${temp_file}_filtered" "$temp_file"

# Step 7: Remove lines ending with a colon
grep -E -v ':$' "$temp_file" > "${temp_file}_filtered" && mv "${temp_file}_filtered" "$temp_file"

# Step 8: Remove blank lines
grep -E -v '^$' "$temp_file" > "${temp_file}_filtered" && mv "${temp_file}_filtered" "$temp_file"

# Step 9: Remove lines containing a double hyphen (--)
grep -E -v '--' "$temp_file" > "${temp_file}_filtered" && mv "${temp_file}_filtered" "$temp_file"

# Step 10: Keep only lines starting with A-Za-z
grep -E '^[A-Za-z]' "$temp_file" > "$output_file"

# Clean up temporary file
rm -f "$temp_file"

# Check if any lines were found and written to the output file
if [ ! -s "$output_file" ]; then
    echo "No matching lines found for selector '$selector'."
    rm -f "$output_file"
    exit 0
fi

echo "Processing complete. Output saved to '$output_file'."
