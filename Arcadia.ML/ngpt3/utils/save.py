import os

def save_dataset(labels_data, filename="/app/output/dataset.txt"):
    """Save the dataset to a TXT file."""
    os.makedirs(os.path.dirname(filename), exist_ok=True)
    with open(filename, "w") as f:
        for label, examples in labels_data.items():
            unique_examples = list(set(examples))
            for example in unique_examples:
                f.write(f"{label}\t{example}\n")
