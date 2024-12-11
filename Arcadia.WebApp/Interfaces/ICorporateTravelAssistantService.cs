using Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;
using System.Threading.Tasks;

namespace Arcadia.WebApp.Interfaces
{
    public interface ICorporateTravelAssistantService
    {
        Task<ApiResponse> SendMessageAsync(string message);
        // Add other method signatures as needed
    }
}
