// CTADetectIntent3.cs
using Arcadia.Shared.Interfaces;
using Arcadia.Shared.Models;
using Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging; // Add this

namespace Arcadia.Shared.Services
{
    public class CTADetectIntent3 : ICTADetectIntent3
    {
        private readonly IStringSearcher2 _stringSearcher2;
        private readonly List<IntentDefinition> _intentDefinitions;
        private readonly ILogger<CTADetectIntent3> _logger; // Add logger

        // Constructor injection of IStringSearcher2, IConfiguration, and ILogger
        public CTADetectIntent3(IStringSearcher2 stringSearcher2, IConfiguration configuration, ILogger<CTADetectIntent3> logger)
        {
            _stringSearcher2 = stringSearcher2;
            _logger = logger;

            // Load intent definitions from intents.json
            var intentsPath = configuration["IntentConfiguration:Path"] ?? "Configurations/intents.json";
            _logger.LogInformation($"Loading intents from: {intentsPath}");

            if (File.Exists(intentsPath))
            {
                try
                {
                    var json = File.ReadAllText(intentsPath);
                    _intentDefinitions = JsonConvert.DeserializeObject<List<IntentDefinition>>(json) ?? new List<IntentDefinition>();
                    _logger.LogInformation($"Loaded {_intentDefinitions.Count} intents.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to load or parse intents.json.");
                    _intentDefinitions = new List<IntentDefinition>();
                }
            }
            else
            {
                _logger.LogWarning($"intents.json not found at path: {intentsPath}");
                _intentDefinitions = new List<IntentDefinition>();
            }
        }

        // Implement the method to return a list of detected intents
        public List<CorporateTravelAssistantIntent2> GetCorporateTravelAssistantIntents3(string input)
        {
            _logger.LogInformation($"Processing input: {input}");
            var detectedIntents = new List<CorporateTravelAssistantIntent2>();

            foreach (var intentDef in _intentDefinitions)
            {
                _logger.LogInformation($"Checking intent: {intentDef.Intent}");
                bool isMatch = _stringSearcher2.Search(
                    input,
                    intentDef.MustHaveWords,
                    intentDef.SearchWords,
                    isContainsAny: true
                );

                if (isMatch)
                {
                    if (Enum.TryParse(intentDef.Intent, out CorporateTravelAssistantIntent2 detectedIntent))
                    {
                        // Avoid duplicates
                        if (!detectedIntents.Contains(detectedIntent))
                        {
                            _logger.LogInformation($"Detected intent: {detectedIntent}");
                            detectedIntents.Add(detectedIntent);
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Failed to parse intent enum for: {intentDef.Intent}");
                    }
                }
                else
                {
                    _logger.LogInformation($"No match for intent: {intentDef.Intent}");
                }
            }

            // If no intent matches, add Unknown
            if (detectedIntents.Count == 0)
            {
                _logger.LogInformation("No intents matched. Adding Unknown.");
                detectedIntents.Add(CorporateTravelAssistantIntent2.Unknown);
            }

            return detectedIntents;
        }
    }
}
