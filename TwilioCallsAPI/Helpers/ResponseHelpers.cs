using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Twilio.TwiML.Voice;

namespace TwilioTests.API.Helpers;

public static class ResponseHelpers
{
    public static List<int> GetNumbersOnly(this string input)
    {
        var result = new List<int>();

        foreach (char c in input)
        {
            if (int.TryParse(c.ToString(), out int parsed))
            {
                result.Add(parsed);
            }
        }

        return result;
    }

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
