// StringSearcher.cs
using Arcadia.Shared.Interfaces;

namespace Arcadia.Shared.Services
{
    public class StringSearcher : IStringSearcher
    {
        public bool Search(string input, IEnumerable<string> wordsToFind, bool isContainsAny = true)
        {
            if (isContainsAny)
            {
                // Check if any of the words are in the input
                return wordsToFind.Any(word => input.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            else
            {
                // Check if all of the words are in the input
                return wordsToFind.All(word => input.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }
    }
}
