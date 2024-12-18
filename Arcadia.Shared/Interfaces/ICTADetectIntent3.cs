// ICTADetectIntent3.cs
using Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;
using System.Collections.Generic;

namespace Arcadia.Shared.Interfaces
{
    public interface ICTADetectIntent3
    {
        List<CorporateTravelAssistantIntent2> GetCorporateTravelAssistantIntents3(string input);
    }
}
