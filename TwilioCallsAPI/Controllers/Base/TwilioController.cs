using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Twilio;
using Twilio.TwiML.Voice;

using TwilioTests.API.Configuration;

namespace TwilioTests.API.Controllers.Base;

[ApiController]
[Route("[controller]")]
public class TwilioController : ControllerBase
{
    protected readonly ILogger<TwilioController> _logger;

    public TwilioController(
        ILogger<TwilioController> logger,
        IOptions<TwilioConfiguration> twilioConfiguration)
    {
        _logger = logger;

        TwilioClient.Init(twilioConfiguration.Value.AccountSID, twilioConfiguration.Value.AuthToken);
    }

    protected Uri Action(string action, string controller = null)
    {
        if (controller is null)
        {
            controller = ControllerContext.ActionDescriptor.ControllerName;
        }
        var baseUrl = $"{ControllerContext.HttpContext.Request.Scheme}://{ControllerContext.HttpContext.Request.Host}";
        var path = $"{baseUrl}/{controller}/{action}";

        return new Uri(path);
    }

    protected Uri Media(string filename)
    {
        return Action(filename, "Media");
    }

    protected Gather Gather(int numDigits, string action, string controller = null)
    {
        return new Gather(
            numDigits: numDigits,
            action: Action(action, controller)
        );
    }
}
