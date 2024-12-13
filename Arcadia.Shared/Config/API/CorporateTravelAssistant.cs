namespace Arcadia.Shared.Config.API;

public static class CorporateTravelAssistantApiEndpoints
{
    public const string SendMessage = "corporate-travel-assistant/sendMessage";

    // Intent: Book a Flight
    public static readonly string[] mustHaveWords_Flight = ["book", "flight"];
    public static readonly string[] searchWords_Flight = ["book", "flight", "reserve", "schedule", "ticket"];

    // Intent: Book a Car Hire
    public static readonly string[] mustHaveWords_CarHire = ["book", "car", "hire"];
    public static readonly string[] searchWords_CarHire = ["car", "hire", "rent", "vehicle", "automobile"];

    // Intent: Book Accommodation
    public static readonly string[] mustHaveWords_Accommodation = ["book", "hotel"];
    public static readonly string[] searchWords_Accommodation = ["hotel", "accommodation", "stay", "room", "lodging"];
}