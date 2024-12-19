import json
import nltk
from utils.generate import generate_paraphrases
from utils.augment import synonym_replacement, introduce_typos
from utils.save import save_dataset

nltk.download('wordnet')
nltk.download('omw-1.4')

# Load labels_data from the JSON file
with open("/app/labels_data.json", "r") as f:
    labels_data = json.load(f)

# Generate 1000 examples per label
target_examples_per_label = 1000
initial_examples_per_label = len(next(iter(labels_data.values())))
paraphrases_needed_per_example = target_examples_per_label // initial_examples_per_label

# Generate paraphrases for each label
for label, examples in labels_data.items():
    print(f"Generating paraphrases for label: {label}")
    all_paraphrases = []
    for example in examples:
        all_paraphrases.extend(generate_paraphrases(example, num_paraphrases=paraphrases_needed_per_example))
    
    # Filter out empty paraphrases
    all_paraphrases = [p for p in all_paraphrases if p.strip()]
    
    # Apply synonym replacement and typos to paraphrases only
    augmented_examples = [synonym_replacement(ex) for ex in all_paraphrases]
    typo_examples = [introduce_typos(ex) for ex in all_paraphrases]
    
    labels_data[label].extend(all_paraphrases + augmented_examples + typo_examples)
    print(f"Completed generating data for label: {label}")

# Save the dataset
save_dataset(labels_data)
