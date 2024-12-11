using Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;

namespace Arcadia.WebApp.Interfaces;

public interface ICorporateTravelAssistantService
{
    Task<ApiResponse> SendMessageAsync(string message);
}