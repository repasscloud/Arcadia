#!/usr/bin/env bash

docker volume create ollama-local
docker compose up -d
docker exec -it ngpt-ollama-ollama-1 ollama pull llama3.1

curl -X POST http://localhost:11434/api/generate \
  -d '{
        "model": "llama3.1",
        "prompt": "Generate 40 statements that would be used by someone using a corporate travel chatbot to book a flight. Only return the statements, no explanations or groupings.",
        "stream": false
      }' -o output.json
