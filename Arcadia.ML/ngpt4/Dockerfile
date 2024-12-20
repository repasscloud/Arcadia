# Dockerfile
FROM ubuntu:22.04

ENV TZ=Australia/Sydney
ENV DEBIAN_FRONTEND=noninteractive

# Install dependencies
RUN --mount=type=cache,target=/var/cache/apt,sharing=locked \
    apt-get update && apt-get install -y \
    python3 \
    python3-pip \
    wget \
    unzip \
    && rm -rf /var/lib/apt/lists/*

# Install GPT4All Python package
RUN pip install --no-cache-dir gpt4all

# Set working directory
WORKDIR /app

# Copy the Python script into the container
COPY ./run_gpt4all.py /app/run_gpt4all.py

# Download a CPU-compatible GPT4All model
RUN wget https://gpt4all.io/models/gguf/gpt4all-falcon-q4_0.gguf -O /app/gpt4all-falcon-q4_0.gguf

# Command to run the script
CMD ["python3", "/app/run_gpt4all.py"]
