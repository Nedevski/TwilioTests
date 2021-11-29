using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Twilio.TwiML.Voice;

using TwilioTests.API.Configuration;
using TwilioTests.API.Controllers.Base;
using TwilioTests.API.Helpers;

namespace TwilioTests.API.Controllers;

public class TestController : TwilioController
{
    private const string TEST_CALL = "TestCall";

    private readonly NumbersConfiguration _numbersConfig;

    public TestController(
        ILogger<OutgoingCallController> logger,
        IOptions<TwilioConfiguration> twilioConfiguration,
        IOptions<NumbersConfiguration> numbersConfiguration) : base(logger, twilioConfiguration)
    {
        _numbersConfig = numbersConfiguration.Value;
    }

    [HttpPost]
    [Route(TEST_CALL)]
    public IActionResult TestCall(string phoneNumber = null)
    {
        if (phoneNumber is null)
        {
            phoneNumber = _numbersConfig.BGNikola;
        }

        var voice = new VoiceResponse()
            .Play(Media("f3fe3e3d9f854b68a1007eafe85a5189.mp3"))
            // long gather to record the call after the message has been played
            .Gather(timeout: 10, numDigits: 10);

        var call = CallResource.Create(
            twiml: new Twiml(voice.ToString()),
            from: new PhoneNumber(_numbersConfig.DefaultSender),
            to: new PhoneNumber(_numbersConfig.BGNikola),
            record: true
        );

        return Ok(call.Sid);
    }
}
