FROM python:3.9-slim

# Install GPT4All
RUN pip install gpt4all

# Copy your script
COPY main.py /app/main.py
WORKDIR /app

# Command to run the script
CMD ["python", "main.py"]
