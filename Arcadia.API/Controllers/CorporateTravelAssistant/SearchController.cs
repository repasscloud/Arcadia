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

        public SearchController(
            ICTADetectIntent intentDetector,
            ICTADetectIntent2 intentDetector2)
        {
            _intentDetector = intentDetector;
            _intentDetector2 = intentDetector2;
        }

        [HttpGet]
        public IActionResult Search(string input)
        {
            // Detect intents using the services
            CorporateTravelAssistantIntent intent = _intentDetector.GetCorporateTravelAssistantIntent(input);
            CorporateTravelAssistantIntent2 intent2 = _intentDetector2.GetCorporateTravelAssistantIntent2(input);

            // Return the detected intents
            return Ok(new
            {
                DetectedIntent = intent.ToString(),
                DetectedIntent2 = intent2.ToString()
            });
        }
    }
}
