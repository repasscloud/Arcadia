namespace Arcadia.Shared.Models.WebApp.SysLib;

public class ApiSettings
{
    public ApiEndpointSettings CorporateTravelAssistant { get; set; } = new ApiEndpointSettings();
    public ApiEndpointSettings AnotherService { get; set; } = new ApiEndpointSettings();
}
