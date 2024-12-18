// CTADetectIntent.cs
using Arcadia.Shared.Interfaces;
using Arcadia.Shared.Models;
using Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Arcadia.Shared.Services
{
    public class CTADetectIntent : ICTADetectIntent
    {
        private readonly IStringSearcher _stringSearcher;
        private readonly List<IntentDefinition> _intentDefinitions;

        // Constructor injection of IStringSearcher and IConfiguration
        public CTADetectIntent(IStringSearcher stringSearcher, IConfiguration configuration)
        {
            _stringSearcher = stringSearcher;

            // Load intent definitions from intents.json
            var intentsPath = configuration.GetValue<string>("IntentConfiguration:Path") ?? "Configurations/intents.json";
            if (File.Exists(intentsPath))
            {
                var json = File.ReadAllText(intentsPath);
                _intentDefinitions = JsonConvert.DeserializeObject<List<IntentDefinition>>(json);
            }
            else
            {
                _intentDefinitions = new List<IntentDefinition>();
            }
        }

        public CorporateTravelAssistantIntent GetCorporateTravelAssistantIntent(string input)
        {
            foreach (var intentDef in _intentDefinitions)
            {
                bool isMatch = _stringSearcher.Search(
                    input,
                    intentDef.SearchWords,
                    isContainsAny: true
                );

                if (isMatch)
                {
                    if (Enum.TryParse(intentDef.Intent, out CorporateTravelAssistantIntent detectedIntent))
                    {
                        return detectedIntent;
                    }
                }
            }

            // If no intent matches, return Unknown
            return CorporateTravelAssistantIntent.Unknown;
        }
    }
}
