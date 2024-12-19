import random
from transformers import pipeline
from nltk.corpus import wordnet
import nltk
import pandas as pd
import os

nltk.download('wordnet')
nltk.download('omw-1.4')

# Initial labeled data
labels_data = {
    "NewBooking_SelectFlight": [
        "I want to book a flight to New York.",
        "Can you help me book a flight to London?",
        "I'd like to purchase a plane ticket to Tokyo.",
        "Book me a flight to Paris.",
        "I need to reserve a flight for my trip to Sydney.",
        "Help me find a flight to Berlin.",
        "I want to get a flight ticket to Dubai.",
        "Looking to book a flight to Rome.",
        "Can I book a flight to Madrid?",
        "I'd like to arrange a flight to Amsterdam.",
        "I need to arrange a flight for my vacation.",
        "Help me book a ticket to Chicago.",
        "I'd like to find a flight to San Francisco.",
        "Book a flight for me to Toronto.",
        "Can you search for flights to Miami?",
        "I want to schedule a flight to Boston.",
        "Looking to book a plane ticket to Seattle.",
        "Can I get a flight to Houston?",
        "I'd like to reserve a seat on a flight to Atlanta.",
        "Help me find a cheap flight to Orlando.",
        "I need to book a flight to Los Angeles.",
        "Could you help me find a ticket to Vancouver?",
        "I want to buy a flight to Dublin.",
        "Searching for a flight to Auckland.",
        "I'd like to get a plane ticket to Prague.",
        "Can you arrange a flight to Singapore?",
        "Find me a flight to Stockholm.",
        "I need a reservation for a flight to Copenhagen.",
        "Help me secure a flight to Cairo.",
        "I'm looking to purchase a flight to Buenos Aires.",
        "Book me a flight heading to Geneva.",
        "I want to travel to Montreal by air.",
        "Can we find a flight option to Shanghai?",
        "I need to arrange a ticket to Hong Kong.",
        "Please look for a flight to Rio de Janeiro.",
        "I'd like a direct flight to Oslo.",
        "Can I get a flight to Johannesburg?",
        "Help me book airfare to Helsinki.",
        "I want to fly to Mumbai.",
        "Search for a ticket to New Delhi, please.",
        "I'm aiming to book a flight to Lisbon.",
        "Could you find me a flight to Brussels?",
        "I need to fly to Warsaw.",
        "Please get me a reservation to Istanbul.",
        "Check for a flight to Budapest.",
        "I'd like to purchase airfare to Tel Aviv.",
        "Can you look up flights to Athens?",
        "I want to arrange travel to Frankfurt by plane.",
        "I need a flight to Zurich.",
        "Find me a booking for a flight to Vienna.",
        "Help me locate a flight to Milan.",
        "I'm looking for flights to Geneva.",
        "Book me a trip to Venice by air.",
        "I want a flight to Warsaw this weekend.",
        "Could we get a ticket to Edinburgh?",
        "I need to schedule a flight to Reykjavik.",
        "Check available flights to Nairobi.",
        "I'd like a flight to Manila.",
        "Find me an airline ticket to Bangkok.",
        "Can you arrange travel to Kuala Lumpur?",
        "I want to book a flight to Jakarta.",
        "Please secure me a seat to Seoul.",
        "I need a ticket to Taipei.",
        "Look for a flight to Ho Chi Minh City.",
        "Could you find flights to Hong Kong tomorrow?",
        "I'm interested in booking a flight to Melbourne.",
        "I want to fly to Perth.",
        "Check for flights going to Lima.",
        "I need to book a plane ticket to Bogota.",
        "Find me an option for a flight to Santiago.",
        "Please arrange a flight to Mexico City.",
        "I'd like to purchase airfare to Cancun.",
        "Can we find a flight heading to Boston?",
        "Get me a seat on a plane to Washington D.C.",
        "I need to book a flight heading to San Diego.",
        "Look for flights traveling to Phoenix.",
        "Reserve me a flight to Philadelphia.",
        "I want a plane ticket to Denver.",
        "Check the availability of flights to Charlotte.",
        "I need to fly into Detroit.",
        "Please find me a route to Minneapolis.",
        "Locate a flight going to Kansas City.",
        "I'd like to fly to New Orleans.",
        "Search for available seats to Nashville.",
        "Can you book me a flight to Portland?",
        "Help me secure a ticket to Austin.",
        "I want to arrange a flight to San Jose.",
        "Find flights heading to Salt Lake City.",
        "I need a booking for a flight to Indianapolis.",
        "Check flights available to Columbus.",
        "I'd like to purchase a ticket to Raleigh.",
        "Can you find a flight to Memphis?",
        "Book me a trip to Milwaukee by air.",
        "I want to fly to Cincinnati.",
        "Look up flights to Sacramento.",
        "Help me get a seat to Pittsburgh.",
        "Arrange a flight for me to Kansas City.",
        "I need to go to Cleveland by plane.",
        "Find me airfare to St. Louis.",
        "Check for flights to Orlando this week.",
        "I'd like to fly to Tampa.",
        "Search for flights going to San Antonio.",
        "Book a flight heading to Baltimore.",
        "Can you get me a flight to Richmond?",
        "I want a plane ticket to Albuquerque.",
        "Look into a flight for me to Honolulu.",
        "Find me a flight to Anchorage.",
        "Please arrange a flight to Omaha.",
        "I need to book a flight to Boise.",
        "Check flights going to Spokane.",
        "I'd like to secure a seat to Reno.",
        "Can we look up a flight to Buffalo?",
        "Book me a flight to Calgary.",
        "Help me find a route to Edmonton by plane.",
        "I want to get to Halifax by air.",
        "Find me an itinerary to Winnipeg.",
        "Arrange a flight to Ottawa, please.",
        "I need to purchase a ticket to Quebec City.",
    ],
    "NewBooking_SelectHotel": [
        "I need to book a hotel in New York.",
        "Can you help me reserve a hotel room?",
        "I'd like to find a hotel for my stay.",
        "Book me a hotel in London.",
        "I want to reserve a hotel in Tokyo.",
        "Looking to book a hotel room.",
        "Can I book a hotel for my vacation?",
        "I'd like to arrange a hotel booking.",
        "Help me find a hotel to stay at.",
        "Book a hotel room for me, please.",
        "I need to reserve a suite at your hotel.",
        "Can you help me find a hotel room?",
        "I'd like to book a double room in your hotel.",
        "Book a single room for my stay.",
        "I want to reserve a deluxe room.",
        "Looking to book a hotel near the airport.",
        "Can I get a room with a sea view?",
        "I'd like to arrange a hotel room for three nights.",
        "Help me find a budget-friendly hotel.",
        "Book a non-smoking room for me.",
        "I need a quiet room away from the elevator.",
        "Can you reserve a hotel close to the conference center?",
        "I'd like to find a room with a king-size bed.",
        "Book me a hotel that allows late check-in.",
        "I want to stay in a hotel with free breakfast.",
        "Looking to book a hotel with a gym.",
        "Can I get a room with early check-in?",
        "I'd like a hotel that offers airport shuttle services.",
        "Help me find a hotel with a swimming pool.",
        "Book a room in a boutique hotel.",
        "I need to reserve a hotel near public transport.",
        "Can you help me find a hotel with conference rooms?",
        "I'd like to book a room with a balcony.",
        "Book a hotel suite with a separate living area.",
        "I want to reserve a hotel near local attractions.",
        "Looking to book a hotel with Wi-Fi included.",
        "Can I get a room on a high floor with a view?",
        "I'd like a hotel that offers complimentary parking.",
        "Help me find a hotel that accepts corporate rates.",
        "Book a hotel that provides laundry services.",
        "I need a hotel room that's wheelchair accessible.",
        "Can you reserve a pet-friendly hotel room?",
        "I'd like to find a hotel with a good business lounge.",
        "Book me a hotel near a major landmark.",
        "I want a room with a work desk and chair.",
        "Looking to book a long-term hotel stay.",
        "Can I find a hotel offering flexible cancellation?",
        "I'd like a room with soundproof walls.",
        "Help me secure a hotel room for next week.",
        "Book a hotel that's highly rated by other travelers.",
        "I need a hotel with on-site dining options.",
        "Can you help me find a hotel that's eco-friendly?",
        "I'd like to book a room near a good restaurant area.",
        "Book me a hotel that's close to shopping centers.",
        "I want to reserve a hotel known for its quiet atmosphere.",
        "Looking to book a room in a luxury hotel.",
        "Can I get a hotel offering business amenities like meeting rooms?",
        "I'd like a hotel room with complimentary toiletries.",
        "Help me find a hotel with a late checkout option.",
        "Book a hotel that offers an executive lounge.",
        "I need a hotel with a fitness center.",
        "Can you reserve a hotel that's close to my client's office?",
        "I'd like to find a hotel that allows contactless check-in.",
        "Book a room in a hotel that's family-friendly.",
        "I want to stay in a hotel that's known for great service.",
        "Looking to book a hotel with multilingual staff.",
        "Can I find a hotel that includes a complimentary minibar?",
        "I'd like a hotel with a rooftop bar.",
        "Help me get a room in a well-reviewed hotel.",
        "Book a hotel room with flexible check-in times.",
        "I need to reserve a hotel that's close to public parks.",
        "Can you help me find a hotel with free cancellation?",
        "I'd like to book a room in a trendy boutique hotel.",
        "Book me a hotel known for excellent breakfast options.",
        "I want to reserve a hotel with easy highway access.",
        "Looking for a hotel that's close to the train station.",
        "Can I get a room in a hotel with a good loyalty program?",
        "I'd like to arrange a stay in a hotel with business facilities.",
        "Help me find a hotel that offers room service 24/7.",
        "Book a hotel that provides a shuttle to the convention center.",
        "I need a hotel with a secure parking area.",
        "Can you reserve a hotel room with a kitchenette?",
        "I'd like to book a room that's newly renovated.",
        "Book me a hotel with in-room safes.",
        "I want to stay at a hotel that's well-located for sightseeing.",
        "Looking for a hotel that can provide extra pillows and blankets.",
        "Can I find a hotel that's known for comfortable beds?",
        "I'd like a hotel that allows late arrivals.",
        "Help me secure a hotel near the business district.",
        "Book a hotel that's close to local restaurants.",
        "I need a room in a hotel that has good reviews on cleanliness.",
        "Can you help me find a hotel with excellent Wi-Fi?",
        "I'd like to book a room that's spacious enough for work.",
        "Book me a hotel that's affordable and central.",
        "I want to reserve a hotel near the city center.",
        "Looking for a hotel that's within my company's budget limit.",
        "Can I get a hotel that offers an airport pickup service?",
        "I'd like a hotel with a recognized brand name.",
        "Help me find a hotel with good security measures.",
        "Book a hotel with a restaurant on-site.",
        "I need a hotel room that's cozy and well-lit.",
        "Can you reserve a hotel near the conference venue?",
        "I'd like to book a room with late check-out availability.",
        "Book me a hotel offering package deals.",
        "I want to stay in a hotel that's known for quiet nights.",
        "Looking for a hotel that can handle special requests.",
        "Can I find a hotel that's popular among business travelers?",
        "I'd like a hotel that's flexible with changes in reservations.",
        "Help me choose a hotel that has a business center.",
        "Book a hotel that can arrange wake-up calls.",
        "I need a room in a hotel that's easy to navigate.",
        "Can you help me find a hotel that's good for long stays?",
        "I'd like to book a hotel near cultural attractions.",
        "Book me a hotel that accepts my corporate discount.",
        "I want a hotel that provides complimentary bottled water.",
        "Looking for a hotel that has a nice lobby area.",
        "Can I get a hotel that's known for good customer service?",
        "I'd like a hotel room with adjustable temperature controls.",
        "Help me find a hotel that's suitable for a short stay.",
        "Book a hotel that offers daily housekeeping.",
        "I need a hotel that can handle my late arrival time.",
        "Can you reserve a hotel room close to my meeting location?",
        "I'd like to book a room in a hotel that's newly opened.",
        "Book me a hotel known for its comfortable amenities.",
        "I want to stay in a hotel that's quiet during the night.",
        "Looking to book a hotel that's known for reliability.",
        "Can I get a room in a hotel with complimentary breakfast?",
        "I'd like a hotel that's easy to reach by taxi.",
        "Help me secure a hotel that fits my travel policy.",
        "Book a hotel that's convenient for my business appointments.",
        "I need a hotel room that's well-insulated against noise.",
        "Can you help me find a hotel with large work desks?",
        "I'd like to book a room that overlooks the city skyline.",
        "Book me a hotel that's known for its professional staff.",
        "I want to reserve a hotel that has a spa facility.",
        "Looking for a hotel that offers flexible stay dates.",
        "Can I find a hotel that's near a public park?",
        "I'd like a hotel that can provide rollaway beds.",
        "Help me find a hotel that's suitable for a solo traveler.",
        "Book a hotel that accepts last-minute reservations.",
        "I need a room in a hotel that's been recommended by colleagues.",
        "Can you reserve a hotel that accommodates early departures?",
        "I'd like to book a hotel near a major transportation hub.",
        "Book me a hotel that's renowned for cleanliness.",
        "I want to stay in a hotel offering personalized service.",
        "Looking to book a hotel with a rooftop terrace.",
        "Can I get a hotel that provides room upgrades if available?",
        "I'd like a hotel that's used frequently by business travelers.",
        "Help me choose a hotel that's well-established.",
        "Book a hotel that has a fast check-in process.",
        "I need a hotel room with strong Wi-Fi for work.",
        "Can you help me find a hotel that's not too expensive?",
        "I'd like to book a room that comes with breakfast included.",
        "Book me a hotel that's safe and secure.",
        "I want to reserve a hotel that's easily accessible by public transport.",
        "Looking for a hotel that's suitable for an extended stay.",
        "Can I find a hotel that offers a good loyalty program?",
        "I'd like a hotel that can handle group bookings.",
        "Help me find a hotel that matches my company's travel policy.",
        "Book a hotel that's near a popular convention center.",
        "I need a hotel room that's close to dining options.",
        "Can you reserve a hotel that's known for business facilities?",
        "I'd like to book a room with room service available.",
        "Book me a hotel with an indoor pool.",
        "I want to stay in a hotel that offers shuttle service.",
        "Looking to book a hotel that's good for short business trips.",
        "Can I get a hotel that provides a comfortable work area?",
        "I'd like a hotel with late-night check-in possibilities.",
        "Help me secure a hotel that fits my budget.",
        "Book a hotel that's quiet and peaceful.",
        "I need a hotel room for a weekend trip.",
        "Can you help me find a hotel that's highly recommended?",
        "I'd like to book a room that's suitable for a single traveler.",
        "Book me a hotel that's reliable and efficient.",
        "I want to reserve a hotel known for good amenities.",
        "Looking for a hotel that's near a conference I'm attending.",
        "Can I find a hotel that's popular with corporate clients?",
        "I'd like a hotel room with flexible payment options.",
        "Help me find a hotel that allows early check-in.",
        "Book a hotel that offers some complimentary services.",
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
    
    # Apply synonym replacement and typos to paraphrases only
    augmented_examples = [synonym_replacement(ex) for ex in all_paraphrases]
    typo_examples = [introduce_typos(ex) for ex in all_paraphrases]
    
    labels_data[label].extend(all_paraphrases + augmented_examples + typo_examples)
    print(f"Completed generating data for label: {label}")


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
    os.makedirs(os.path.dirname(filename), exist_ok=True)
    with open(filename, "w") as f:
        for label, examples in labels_data.items():
            unique_examples = list(set(examples))
            random.shuffle(unique_examples)
            for example in unique_examples:
                f.write(f"{label}\t{example}\n")


save_dataset(labels_data)
