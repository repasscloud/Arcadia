namespace Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;

public class Message
{
    public string Content { get; set; } = string.Empty;
    public bool IsSystem { get; set; }
    public bool IsLoading { get; set; } = false;
}