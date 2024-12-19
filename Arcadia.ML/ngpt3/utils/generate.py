from transformers import pipeline

# Load the paraphrase pipeline with a CPU-friendly model
paraphrase_pipeline = pipeline("text2text-generation", model="t5-small", device=-1)

def generate_paraphrases(text, num_paraphrases=5):
    """Generate multiple paraphrases for a given text."""
    paraphrases = paraphrase_pipeline(
        f"paraphrase: {text}",
        max_length=50,
        num_return_sequences=num_paraphrases,
        do_sample=True,
        top_k=50,
        top_p=0.95,
        temperature=0.7
    )
    return [p['generated_text'] for p in paraphrases]
