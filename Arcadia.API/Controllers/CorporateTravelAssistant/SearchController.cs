// SearchController.cs
using Arcadia.Shared.Interfaces;
using Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;
using Microsoft.AspNetCore.Mvc;

namespace Arcadia.API.CorporateTravelAssistant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ICTADetectIntent _intentDetector;
        private readonly ICTADetectIntent2 _intentDetector2;
        private readonly IStringSearcher _stringSearcher;
        private readonly IStringSearcher2 _stringSearcher2;

        public SearchController(
            ICTADetectIntent intentDetector,
            ICTADetectIntent2 intentDetector2,
            IStringSearcher stringSearcher,
            IStringSearcher2 stringSearcher2)
        {
            _intentDetector = intentDetector;
            _intentDetector2 = intentDetector2;
            _stringSearcher = stringSearcher;
            _stringSearcher2 = stringSearcher2;
        }

        [HttpGet]
        public IActionResult Search(string input)
        {
            // Detect intent using CTADetectIntent service
            CorporateTravelAssistantIntent intent = _intentDetector.GetCorporateTravelAssistantIntent(input);

            // Detect intent using CTADetectIntent2 service
            CorporateTravelAssistantIntent2 intent2 = _intentDetector2.GetCorporateTravelAssistantIntent2(input);

            // Example usage of StringSearcherService directly
            var mustHave = new List<string> { "book", "flight" };
            var searchWords = new List<string> { "new", "ticket" };
            bool searchResult = _stringSearcher.Search(input, searchWords, isContainsAny: true);

            // Example usage of StringSearcher2Service directly
            var mustHave2 = new List<string> { "book", "flight" };
            var searchWords2 = new List<string> { "new", "ticket" };
            bool searchResult2 = _stringSearcher2.Search(input, mustHave2, searchWords2, isContainsAny: true);

            // Return results based on intents and search results
            return Ok(new
            {
                DetectedIntent = intent.ToString(),
                DetectedIntent2 = intent2.ToString(),
                SearchResult = searchResult,
                SearchResult2 = searchResult2
            });
        }
    }
}
