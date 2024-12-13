// CTADetectIntent.cs
using Arcadia.Shared.Interfaces;
using Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;

namespace Arcadia.Shared.Services
{
    public class CTADetectIntent : ICTADetectIntent
    {
        private readonly IStringSearcher _stringSearcher;

        // Constructor injection of IStringSearcher
        public CTADetectIntent(IStringSearcher stringSearcher)
        {
            _stringSearcher = stringSearcher;
        }

        public CorporateTravelAssistantIntent GetCorporateTravelAssistantIntent(string input)
        {
            // Define keyword lists for different intents
            var bookFlightWords = new List<string> { "book", "flight", "reserve", "schedule", "ticket" };

            // Use StringSearcher to detect if the intent is to book a flight
            bool isBookFlight = _stringSearcher.Search(input, new List<string> { "book", "flight" }, isContainsAny: true);
            if (isBookFlight)
            {
                return CorporateTravelAssistantIntent.BookFlight;
            }

            // Add other intent detections using IStringSearcher as needed

            // Default to Unknown if no intent matches
            return CorporateTravelAssistantIntent.Unknown;
        }
    }
}
