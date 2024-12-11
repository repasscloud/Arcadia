using Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;
using Arcadia.WebApp.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Arcadia.WebApp.Services;

public class CorporateTravelAssistantService : ICorporateTravelAssistantService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CorporateTravelAssistantService> _logger;

    public CorporateTravelAssistantService(HttpClient httpClient, ILogger<CorporateTravelAssistantService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ApiResponse> SendMessageAsync(string message)
    {
        var payload = new { input = message };

        try
        {
            _logger.LogInformation("Sending message to Corporate Travel Assistant API: {Message}", message);

            // Send POST request to the API
            var response = await _httpClient.PostAsJsonAsync("", payload);

            // Check if the response indicates success
            if (response.IsSuccessStatusCode)
            {
                // Attempt to deserialize the response content
                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

                if (apiResponse != null)
                {
                    _logger.LogInformation("Received successful response from API.");
                    return apiResponse;
                }
                else
                {
                    _logger.LogWarning("API response deserialized to null.");
                    return new ApiResponse
                    {
                        Response = "Received an empty response from the server.",
                        Buttons = new List<ApiButton>()
                    };
                }
            }
            else
            {
                // Log the unsuccessful status code and reason
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("API request failed with status code {StatusCode}: {ReasonPhrase}. Content: {Content}",
                    response.StatusCode, response.ReasonPhrase, errorContent);

                return new ApiResponse
                {
                    Response = $"Error: {response.ReasonPhrase}",
                    Buttons = new List<ApiButton>()
                };
            }
        }
        catch (HttpRequestException httpEx)
        {
            // Handle HTTP request specific exceptions
            _logger.LogError(httpEx, "HTTP request to Corporate Travel Assistant API failed.");
            return new ApiResponse
            {
                Response = "Unable to reach the server. Please try again later.",
                Buttons = new List<ApiButton>()
            };
        }
        catch (NotSupportedException notSupEx)
        {
            // Handle content type not supported exceptions
            _logger.LogError(notSupEx, "The content type is not supported.");
            return new ApiResponse
            {
                Response = "Unsupported response format received.",
                Buttons = new List<ApiButton>()
            };
        }
        catch (Exception ex)
        {
            // Handle all other exceptions
            _logger.LogError(ex, "An unexpected error occurred while sending the message.");
            return new ApiResponse
            {
                Response = "An unexpected error occurred. Please try again later.",
                Buttons = new List<ApiButton>()
            };
        }
    }
}
