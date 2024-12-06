#!/bin/bash

# Define an associative array of services and their Dockerfiles
declare -A services=(
    ["arcadia.api"]="Dockerfile.API"
    ["arcadia.webapp"]="Dockerfile.WebApp"
)

# Iterate over the services and build each one
for service in "${(@k)services}"; do
    echo "Removing docker image $service"
    docker rmi $service --force
    dockerfile=${services[$service]}
    echo "Building $service with $dockerfile..."
    docker build --rm --file ./$dockerfile -t $service .
    if [[ $? -ne 0 ]]; then
        echo "Error: Failed to build $service" >&2
        exit 1
    fi
    echo "$service built successfully."
done

echo "All services built successfully."
