// IStringSearcher.cs
namespace Arcadia.Shared.Interfaces
{
    public interface IStringSearcher
    {
        bool Search(string input, IEnumerable<string> wordsToFind, bool isContainsAny = true);
    }

    public interface IStringSearcher2
    {
        bool Search(string input, IEnumerable<string> mustHaveWords, IEnumerable<string> searchWords, bool isContainsAny = true);
    }
}
