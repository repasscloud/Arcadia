#!/usr/bin/env bash

# Stop and remove all running containers
docker compose down 2>/dev/null
docker stop $(docker ps -aq) 2>/dev/null
docker rm $(docker ps -aq) 2>/dev/null

# Remove all images
docker rmi $(docker images -q) -f 2>/dev/null

# Remove all volumes
docker volume rm $(docker volume ls -q) -f 2>/dev/null

# Remove all networks (except default ones)
docker network rm $(docker network ls | grep -v "bridge\|host\|none" | awk '{if (NR>1) print $1}') 2>/dev/null

# Prune everything (dangling volumes, images, etc.)
docker system prune -a -f 2>/dev/null

# Remove any Docker builder cache
docker builder prune -a -f 2>/dev/null

# Output confirmation message
echo "Docker has been completely cleared."
