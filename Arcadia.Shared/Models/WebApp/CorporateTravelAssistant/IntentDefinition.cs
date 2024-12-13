// IntentDefinition.cs
namespace Arcadia.Shared.Models
{
    public class IntentDefinition
    {
        public required string Intent { get; set; }
        public required List<string> MustHaveWords { get; set; }
        public required List<string> SearchWords { get; set; }
    }
}
