// CTADetectIntent2.cs
using Arcadia.Shared.Interfaces;
using Arcadia.Shared.Models;
using Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Arcadia.Shared.Services
{
    public class CTADetectIntent2 : ICTADetectIntent2
    {
        private readonly IStringSearcher2 _stringSearcher2;
        private readonly List<IntentDefinition> _intentDefinitions;

        // Constructor injection of IStringSearcher2 and IConfiguration
        public CTADetectIntent2(IStringSearcher2 stringSearcher2, IConfiguration configuration)
        {
            _stringSearcher2 = stringSearcher2;

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

        public CorporateTravelAssistantIntent2 GetCorporateTravelAssistantIntent2(string input)
        {
            foreach (var intentDef in _intentDefinitions)
            {
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
                        return detectedIntent;
                    }
                }
            }

            // If no intent matches, return Unknown
            return CorporateTravelAssistantIntent2.Unknown;
        }
    }
}
