namespace Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;

public class ApiResponse
{
    public string Response { get; set; } = string.Empty;
    public List<ApiButton> Buttons { get; set; } = new List<ApiButton>();
}