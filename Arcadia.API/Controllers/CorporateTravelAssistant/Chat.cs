using Arcadia.Shared.Config.API;
using Arcadia.Shared.Interfaces;
using Arcadia.Shared.Models.WebApp.CorporateTravelAssistant;
using Microsoft.AspNetCore.Mvc;

namespace Arcadia.API.CorporateTravelAssistant.Controllers;

[ApiController]
[Route($"api/{CorporateTravelAssistantApiEndpoints.SendMessage}")]
public class ChatController : ControllerBase
{
    private readonly IStringSearcher _stringSearcher;
    private readonly IStringSearcher2 _stringSearcher2;

    public ChatController(IStringSearcher stringSearcher, IStringSearcher2 stringSearcher2)
    {
        _stringSearcher = stringSearcher;
        _stringSearcher2 = stringSearcher2;
    }

    [HttpPost("")]
    public IActionResult GetResponse([FromBody] ChatRequest request)
    {
        var input = request.Input;
        ApiResponse response;

        string[] newBookingWords = new string[] { "new", "flight" };
        bool newBooking = _stringSearcher.Search(input, newBookingWords, true);

        if (newBooking)
        {
            response = new ApiResponse
            {
                Response = "xxxxx? Flights, Accommodation, or Car Hire?",
                Buttons = new List<ApiButton>
                {
                    new ApiButton { Text = "Flights", Action = "Book Flights" },
                    new ApiButton { Text = "Accommodation", Action = "Book Accommodation" },
                    new ApiButton { Text = "Car Hire", Action = "Book Car Hire" }
                }
            };

            return Ok(response);
        }

        if (input == "Create a new booking")
        {
            response = new ApiResponse
            {
                Response = "What would you like to book? Flights, Accommodation, or Car Hire?",
                Buttons = new List<ApiButton>
                {
                    new ApiButton { Text = "Flights", Action = "Book Flights" },
                    new ApiButton { Text = "Accommodation", Action = "Book Accommodation" },
                    new ApiButton { Text = "Car Hire", Action = "Book Car Hire" }
                }
            };
        }
        else if (input == "Speak to a human")
        {
            response = new ApiResponse
            {
                Response = "Connecting you to a human agent...",
                Buttons = new List<ApiButton>()
            };
        }
        else
        {
            response = new ApiResponse
            {
                Response = "I'm not sure how to help with that. Please choose an option below.",
                Buttons = new List<ApiButton>
                {
                    new ApiButton { Text = "Back to Main Menu", Action = "Back to Main Menu" }
                }
            };
        }

        return Ok(response);
    }
}