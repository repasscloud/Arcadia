# syntax=docker/dockerfile:1

# Base image
FROM --platform=$BUILDPLATFORM python:3.9-slim AS base

WORKDIR /app

# This is the architecture you’re building for, which is passed in by the builder.
ARG TARGETARCH

# Install dependencies
RUN apt-get update && apt-get install -y \
    libgomp1 \
    wget \
    && rm -rf /var/lib/apt/lists/*

# Install Python libraries
RUN pip install --no-cache-dir transformers torch nltk textattack pandas

# Download NLTK data
RUN python -c "import nltk; nltk.download('wordnet'); nltk.download('omw-1.4')"

# Copy the script into the container
COPY ./main.py /app/main.py

# Command to run the script
CMD ["python", "main.py"]
