using Microsoft.AspNetCore.Mvc;

namespace TwilioTests.API.Helpers;

public static class ResponseHelpers
{
    public static IActionResult AsTwiML(this TwiML response)
    {
        return new ContentResult
        {
            Content = response.ToString(),
            ContentType = "application/xml",
            StatusCode = 200
        };
    }
}
