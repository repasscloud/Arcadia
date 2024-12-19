# syntax=docker/dockerfile:1

# Base image
FROM --platform=$BUILDPLATFORM python:3.9-slim AS base

WORKDIR /app

# Install system dependencies
RUN apt-get update && apt-get install -y \
    libgomp1 \
    wget \
    && rm -rf /var/lib/apt/lists/*

# Upgrade pip
RUN pip install --upgrade pip

# Create a virtual environment and install Python libraries
RUN python -m venv /app/.venv \
    && /app/.venv/bin/pip install --upgrade pip \
    && /app/.venv/bin/pip install --no-cache-dir transformers torch nltk textattack pandas

# Download NLTK data using the virtual environment
RUN /app/.venv/bin/python -c "import nltk; nltk.download('wordnet'); nltk.download('omw-1.4')"

# Copy the code and data into the container
COPY ./main.py /app/main.py
COPY ./utils /app/utils
COPY ./labels_data.json /app/labels_data.json

# Set the virtual environment's Python as the default
ENV PATH="/app/.venv/bin:$PATH"

# Command to run the script
CMD ["python", "main.py"]
