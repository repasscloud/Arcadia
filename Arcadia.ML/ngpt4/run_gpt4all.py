from gpt4all import GPT4All

# Load the model (ensure the file name matches the one downloaded in the Dockerfile)
model_path = "/app/gpt4all-falcon-q4_0.gguf"
model = GPT4All(model_path)

# Simple prompt
prompt = "Explain the water cycle."

# Generate text
output = model.generate(prompt)
print("Generated Output:")
print(output)
