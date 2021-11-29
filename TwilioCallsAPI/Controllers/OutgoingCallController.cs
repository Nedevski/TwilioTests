using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Twilio.TwiML.Voice;

using TwilioTests.API.Configuration;
using TwilioTests.API.Controllers.Base;
using TwilioTests.API.Helpers;

namespace TwilioTests.API.Controllers;

public class OutgoingCallController : TwilioController
{
    private const string START_CALL = "StartCall";
    private const string CONTINUED = "Continued";

    private readonly NumbersConfiguration _numbersConfig;

    public OutgoingCallController(
        ILogger<OutgoingCallController> logger,
        IOptions<TwilioConfiguration> twilioConfiguration,
        IOptions<NumbersConfiguration> numbersConfiguration) : base(logger, twilioConfiguration)
    {
        _numbersConfig = numbersConfiguration.Value;
    }

    [HttpPost]
    [Route(START_CALL)]
    public IActionResult TestCall(string phoneNumber)
    {
        if (phoneNumber is null) phoneNumber = _numbersConfig.BGNikola;

        var gatherInput = new Gather(numDigits: 1, action: Action(CONTINUED))
            .Say("Hello. To continue press a number between 1 and 10");

        var voice = new VoiceResponse().Append(gatherInput);

        var call = CallResource.Create(
            twiml: new Twiml(voice.ToString()),
            from: new PhoneNumber(_numbersConfig.DefaultSender),
            to: new PhoneNumber(phoneNumber),
            record: true
        );

        return Ok(call.Sid);
    }

    [HttpPost]
    [Route(CONTINUED)]
    public IActionResult OrderRecording([FromForm] VoiceRequest request)
    {
        var response = new VoiceResponse()
            .Say($"You pressed the number {request.Digits}")
            .Hangup();

        return response.AsTwiML();
    }
}
