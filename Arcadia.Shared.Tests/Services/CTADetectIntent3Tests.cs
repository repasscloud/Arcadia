// CTADetectIntent3Tests.cs
using Arcadia.Shared.Interfaces;
using Arcadia.Shared.Models;
using Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;
using Arcadia.Shared.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;
using System.IO;
using System.Reflection;

namespace Arcadia.Shared.Tests.Services
{
    public class CTADetectIntent3Tests
    {
        private readonly Mock<IStringSearcher2> _mockStringSearcher2;
        private readonly IConfiguration _configuration;
        private readonly CTADetectIntent3 _intentDetector3;

        public CTADetectIntent3Tests()
        {
            _mockStringSearcher2 = new Mock<IStringSearcher2>();

            // Mock configuration to point to a test intents.json path
            var inMemorySettings = new Dictionary<string, string> {
                {"IntentConfiguration:Path", "Configurations/intents.json"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _intentDetector3 = new CTADetectIntent3(_mockStringSearcher2.Object, _configuration);
        }

        [Fact]
        public void GetCorporateTravelAssistantIntents3_Returns_MultipleIntents()
        {
            // Arrange
            string input = "Can you help me book a car hire and check my flight status?";

            var intents = new List<IntentDefinition>
            {
                new IntentDefinition
                {
                    Intent = "BookCarHire",
                    MustHaveWords = new List<string> { "book", "car", "hire" },
                    SearchWords = new List<string> { "rent", "vehicle", "automobile", "pickup", "dropoff" }
                },
                new IntentDefinition
                {
                    Intent = "CheckFlightStatus",
                    MustHaveWords = new List<string> { "flight", "status" },
                    SearchWords = new List<string> { "current", "time", "delay", "arrival", "departure" }
                }
            };

            // Mock the file reading by setting up the configuration to return the JSON string
            // Since CTADetectIntent3 reads from a file, we need to ensure that the file exists
            // For simplicity, we'll create a temporary file during the test

            string tempPath = Path.GetTempPath();
            string tempFile = Path.Combine(tempPath, "intents.json");
            File.WriteAllText(tempFile, JsonConvert.SerializeObject(intents));

            // Update the configuration to point to the temporary file
            var updatedSettings = new Dictionary<string, string> {
                {"IntentConfiguration:Path", tempFile}
            };

            var updatedConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(updatedSettings)
                .Build();

            // Re-instantiate the intent detector with the updated configuration
            var intentDetector = new CTADetectIntent3(_mockStringSearcher2.Object, updatedConfiguration);

            // Mock the searcher to return true for both intents
            _mockStringSearcher2.Setup(s => s.Search(
                input,
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<IEnumerable<string>>(),
                true))
                .Returns(true);

            // Act
            List<CorporateTravelAssistantIntent2> detectedIntents = intentDetector.GetCorporateTravelAssistantIntents3(input);

            // Cleanup
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }

            // Assert
            Assert.Contains(CorporateTravelAssistantIntent2.BookCarHire, detectedIntents);
            Assert.Contains(CorporateTravelAssistantIntent2.CheckFlightStatus, detectedIntents);
        }

        [Fact]
        public void GetCorporateTravelAssistantIntents3_Returns_Unknown_When_NoMatch()
        {
            // Arrange
            string input = "I would like to know more about your services.";

            // Mock the searcher to return false for all intents
            _mockStringSearcher2.Setup(s => s.Search(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<IEnumerable<string>>(),
                true))
                .Returns(false);

            // Act
            List<CorporateTravelAssistantIntent2> detectedIntents = _intentDetector3.GetCorporateTravelAssistantIntents3(input);

            // Assert
            Assert.Single(detectedIntents);
            Assert.Contains(CorporateTravelAssistantIntent2.Unknown, detectedIntents);
        }
    }
}
