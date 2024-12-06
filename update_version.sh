#!/bin/bash

# Prompt to confirm if the .version file has been updated
read -p "Have you updated the .version file [y/n]? " answer

# Check if the answer is 'y' or 'Y', else exit the script
if [[ ! "$answer" =~ ^[Yy]$ ]]; then
    echo "Please update the .version file first. Exiting..."
    exit 1
fi

# Ensure the script is run from the correct directory
SCRIPT_DIR=$(dirname "$0")
cd "$SCRIPT_DIR" || exit 1

# Check if .version file exists
if [ ! -f ".version" ]; then
    echo ".version file not found!"
    exit 1
fi

# Read the content of .version file
VERSION=$(cat .version)

# Check if CHANGELOG.md exists
if [ ! -f "CHANGELOG.md" ]; then
    echo "CHANGELOG.md file not found!"
    exit 1
fi

# Check if there is exactly one instance of '## [Unreleased]' in CHANGELOG.md
UNRELEASED_COUNT=$(grep -c "## \[Unreleased\]" CHANGELOG.md)

# If there is not exactly one instance, exit and prompt to update the changelog
if [ "$UNRELEASED_COUNT" -ne 1 ]; then
    echo "There must be exactly one instance of '## [Unreleased]' in CHANGELOG.md. Please update the changelog."
    exit 1
fi

# Replace the line ## [Unreleased] with ## [v{VERSION}]
sed -i "s/## \[Unreleased\]/## [v$VERSION]/" CHANGELOG.md

# Stage the changes for commit
git add .

# Commit the changes
git commit -m "v$VERSION"

# Push the changes to the remote repository
git push

# Tag the commit with an annotated tag
git tag -a "v$VERSION" -m "Release version $VERSION"

# Push the tag to the remote repository
git push origin "v$VERSION"

echo "Version $VERSION updated successfully and pushed!"
