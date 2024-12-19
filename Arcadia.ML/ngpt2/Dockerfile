FROM python:3.9-slim

WORKDIR /app

RUN apt -y update && \
    apt -y upgrade && \
    pip install --upgrade pip

# Install GPT4All
RUN pip install textattack

# # Copy your script
# COPY main.py /app/main.py
# WORKDIR /app

# # Command to run the script
# CMD ["python", "main.py"]
