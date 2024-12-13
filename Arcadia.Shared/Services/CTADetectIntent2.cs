
// CTADetectIntent.cs
using Arcadia.Shared.Interfaces;
using Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;

namespace Arcadia.Shared.Services
{
    public class CTADetectIntent2 : ICTADetectIntent2
    {
        private readonly IStringSearcher2 _stringSearcher2;

        // Constructor injection of IStringSearcher2
        public CTADetectIntent2(IStringSearcher2 stringSearcher2)
        {
            _stringSearcher2 = stringSearcher2;
        }

        public CorporateTravelAssistantIntent2 GetCorporateTravelAssistantIntent2(string input)
        {
            // Define keyword lists for different intents
            var bookFlightMustHave = new List<string> { "book", "flight" };
            var bookFlightSearchWords = new List<string> { "book", "flight", "reserve", "schedule", "ticket" };

            // Use StringSearcher2 to detect if the intent is to book a flight
            bool isBookFlight = _stringSearcher2.Search(input, bookFlightMustHave, bookFlightSearchWords, isContainsAny: true);
            if (isBookFlight)
            {
                return CorporateTravelAssistantIntent2.BookFlight;
            }

            // Add other intent detections using IStringSearcher2 as needed

            // Default to Unknown if no intent matches
            return CorporateTravelAssistantIntent2.Unknown;
        }
    }
}