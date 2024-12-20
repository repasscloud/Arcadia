import ollama
import random
import string

# Configuration
MODEL_NAME = 'mistral'  # Change to your preferred Ollama model
CLASSIFICATION_PROMPT = "Generate a short news headline about technology."
OUTPUT_FILE = 'generated_texts.txt'
NUM_GENERATIONS = 1000

def introduce_typos(text, error_rate=0.05):
    """
    Introduce random typos into the text.
    error_rate: Probability of a character being altered.
    """
    text_list = list(text)
    for i in range(len(text_list)):
        if random.random() < error_rate:
            typo_type = random.choice(['delete', 'swap', 'insert', 'replace'])
            if typo_type == 'delete' and len(text_list) > 1:
                del text_list[i]
            elif typo_type == 'swap' and i < len(text_list) - 1:
                text_list[i], text_list[i + 1] = text_list[i + 1], text_list[i]
            elif typo_type == 'insert':
                text_list.insert(i, random.choice(string.ascii_letters))
            elif typo_type == 'replace':
                text_list[i] = random.choice(string.ascii_letters)
    return ''.join(text_list)

def generate_text(prompt):
    """
    Generate text using the Ollama model.
    """
    response = ollama.generate(model=MODEL_NAME, prompt=prompt)
    return response['response'].strip()

def main():
    with open(OUTPUT_FILE, 'w') as f:
        for i in range(NUM_GENERATIONS):
            try:
                # Step 1: Generate base text
                generated_text = generate_text(CLASSIFICATION_PROMPT)
                
                # Step 2: Introduce typos/spelling differences
                altered_text = introduce_typos(generated_text)
                
                # Step 3: Save to file
                f.write(altered_text + '\n')
                print(f"Generated {i + 1}/{NUM_GENERATIONS}")
            except Exception as e:
                print(f"Error during generation {i + 1}: {e}")

if __name__ == "__main__":
    main()
