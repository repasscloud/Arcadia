# Base image
FROM python:3.11

# Install Ollama (ensure the architecture matches)
RUN curl -fsSL https://ollama.com/install.sh | sh

# Copy the script to the container
COPY ./text_generator.py /app/text_generator.py

# Set the working directory
WORKDIR /app

# Install required Python libraries
RUN pip install requests

# Run the script
CMD ["python", "text_generator.py"]
