from gpt4all import GPT4All

model = GPT4All("gpt4all-falcon-q4_0.gguf")
response = model.generate("Hello, how can I assist you?")
print(response)