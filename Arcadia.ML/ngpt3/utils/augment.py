import random
from nltk.corpus import wordnet

def synonym_replacement(sentence, num_replacements=2):
    """Replace words in the sentence with synonyms."""
    words = sentence.split()
    if not words:  # Check if the list of words is empty
        return sentence
    
    new_sentence = words[:]
    for _ in range(num_replacements):
        word_idx = random.randint(0, len(words) - 1)
        synonyms = wordnet.synsets(words[word_idx])
        if synonyms:
            synonym = synonyms[0].lemmas()[0].name().replace('_', ' ')
            new_sentence[word_idx] = synonym
    return ' '.join(new_sentence)

def introduce_typos(sentence, num_typos=2):
    """Introduce random typos into the sentence."""
    words = sentence.split()
    for _ in range(num_typos):
        word_idx = random.randint(0, len(words) - 1)
        if len(words[word_idx]) > 2:
            char_idx = random.randint(0, len(words[word_idx]) - 1)
            typo_word = list(words[word_idx])
            typo_word[char_idx] = random.choice('abcdefghijklmnopqrstuvwxyz')
            words[word_idx] = ''.join(typo_word)
    return ' '.join(words)
