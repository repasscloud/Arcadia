// Search2Controller.cs
using Arcadia.Shared.Interfaces;
using Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Arcadia.API.CorporateTravelAssistant.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Search2Controller : ControllerBase
    {
        private readonly ICTADetectIntent _intentDetector;
        private readonly ICTADetectIntent2 _intentDetector2;
        private readonly ICTADetectIntent3 _intentDetector3;

        public Search2Controller(
            ICTADetectIntent intentDetector,
            ICTADetectIntent2 intentDetector2,
            ICTADetectIntent3 intentDetector3)
        {
            _intentDetector = intentDetector;
            _intentDetector2 = intentDetector2;
            _intentDetector3 = intentDetector3;
        }

        [HttpGet]
        public IActionResult Search2(string input)
        {
            // Detect single intents using the services
            CorporateTravelAssistantIntent intent = _intentDetector.GetCorporateTravelAssistantIntent(input);
            CorporateTravelAssistantIntent2 intent2 = _intentDetector2.GetCorporateTravelAssistantIntent2(input);

            // Detect multiple intents using the new service
            List<CorporateTravelAssistantIntent2> intents3 = _intentDetector3.GetCorporateTravelAssistantIntents3(input);

            // Return the detected intents
            return Ok(new
            {
                DetectedIntent = intent.ToString(),
                DetectedIntent2 = intent2.ToString(),
                DetectedIntents3 = intents3.Select(i => i.ToString())
            });
        }
    }
}
