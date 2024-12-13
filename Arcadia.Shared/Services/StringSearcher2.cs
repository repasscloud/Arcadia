// StringSearcher2.cs
using Arcadia.Shared.Interfaces;

namespace Arcadia.Shared.Services
{
    public class StringSearcher2 : IStringSearcher2
    {
        public bool Search(string input, IEnumerable<string> mustHaveWords, IEnumerable<string> searchWords, bool isContainsAny = true)
        {
            // Check if all must-have words are present in the input
            bool containsMustHaveWords = mustHaveWords.All(word => input.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0);

            if (!containsMustHaveWords)
            {
                return false; // If must-have words are missing, return false immediately
            }

            // Check search words based on the isContainsAny flag
            if (isContainsAny)
            {
                // At least one of the searchWords must be present
                return searchWords.Any(word => input.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            else
            {
                // All of the searchWords must be present
                return searchWords.All(word => input.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }
    }
}
