import random
from transformers import pipeline
from nltk.corpus import wordnet
import nltk
import pandas as pd

nltk.download('wordnet')
nltk.download('omw-1.4')

# Initial labeled data
labels_data = {
    "booking_flight": [
        "I want to book a flight.",
        "Can you help me book a flight to New York?",
        "I'd like to schedule a plane ticket.",
        "I need to arrange a flight for next week.",
    ],
    "booking_hotel": [
        "I need to book a hotel room.",
        "Can you find me a hotel in Sydney?",
        "I'd like to reserve a hotel for my trip.",
        "Please help me book accommodation.",
    ]
}

# 1. Paraphrase Generation (using T5-small)
paraphrase_pipeline = pipeline("text2text-generation", model="t5-small", device=-1)

def generate_paraphrases(text, num_paraphrases=5):
    # Generate multiple outputs using sampling (with temperature for diversity)
    paraphrases = paraphrase_pipeline(
        f"paraphrase: {text}",
        max_length=50,
        num_return_sequences=num_paraphrases,
        do_sample=True,        # Enable sampling
        top_k=50,              # Consider the top 50 tokens for sampling
        top_p=0.95,            # Nucleus sampling (select tokens with cumulative probability 0.95)
        temperature=0.7        # Control the randomness (lower = more deterministic, higher = more random)
    )
    return [p['generated_text'] for p in paraphrases]


for label, examples in labels_data.items():
    all_paraphrases = []
    for example in examples:
        all_paraphrases.extend(generate_paraphrases(example, num_paraphrases=5))
    labels_data[label].extend(all_paraphrases)

# 2. Synonym Replacement (using WordNet)
def synonym_replacement(sentence, num_replacements=2):
    words = sentence.split()
    new_sentence = words[:]
    for _ in range(num_replacements):
        word_idx = random.randint(0, len(words) - 1)
        synonyms = wordnet.synsets(words[word_idx])
        if synonyms:
            synonym = synonyms[0].lemmas()[0].name().replace('_', ' ')
            new_sentence[word_idx] = synonym
    return ' '.join(new_sentence)

for label, examples in labels_data.items():
    augmented_examples = [synonym_replacement(ex) for ex in examples]
    labels_data[label].extend(augmented_examples)

# 3. Introduce Spelling Errors
def introduce_typos(sentence, num_typos=2):
    words = sentence.split()
    for _ in range(num_typos):
        word_idx = random.randint(0, len(words) - 1)
        if len(words[word_idx]) > 2:
            char_idx = random.randint(0, len(words[word_idx]) - 1)
            typo_word = list(words[word_idx])
            typo_word[char_idx] = random.choice('abcdefghijklmnopqrstuvwxyz')
            words[word_idx] = ''.join(typo_word)
    return ' '.join(words)

for label, examples in labels_data.items():
    typo_examples = [introduce_typos(ex) for ex in examples]
    labels_data[label].extend(typo_examples)

# 4. Save the Dataset to TXT File
def save_dataset(labels_data, filename="/app/output/dataset.txt"):
    with open(filename, "w") as f:
        for label, examples in labels_data.items():
            unique_examples = list(set(examples))
            random.shuffle(unique_examples)
            for example in unique_examples:
                f.write(f"{label}\t{example}\n")

save_dataset(labels_data)
