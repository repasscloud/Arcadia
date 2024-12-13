using Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;

namespace Arcadia.Shared.Interfaces
{
    public interface ICTADetectIntent
    {
        CorporateTravelAssistantIntent GetCorporateTravelAssistantIntent(string input);
    }
}





//bool Search(string input, IEnumerable<string> wordsToFind, bool isContainsAny = true);