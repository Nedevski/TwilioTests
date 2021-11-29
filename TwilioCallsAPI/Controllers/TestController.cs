using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Twilio.TwiML.Voice;

using TwilioTests.API.Configuration;
using TwilioTests.API.Controllers.Base;
using TwilioTests.API.Helpers;

namespace TwilioTests.API.Controllers;

public class TestController : TwilioController
{
    private const string PRANK_CALL = "PrankCall";
    private const string READ_NUMBERS_CALL = "ReadNumbersCall";
    private const string ROUTING_CALL = "RoutingCall";

    private readonly NumbersConfiguration _numbersConfig;

    public TestController(
        ILogger<OutgoingCallController> logger,
        IOptions<TwilioConfiguration> twilioConfiguration,
        IOptions<NumbersConfiguration> numbersConfiguration) : base(logger, twilioConfiguration)
    {
        _numbersConfig = numbersConfiguration.Value;
    }

    [HttpPost]
    [Route(PRANK_CALL)]
    public IActionResult TestCall(string phoneNumber = null)
    {
        if (phoneNumber is null) phoneNumber = _numbersConfig.BGNikola;

        var voice = new VoiceResponse()
            .Play(Media("f3fe3e3d9f854b68a1007eafe85a5189.mp3"))
            // long gather to record the call after the message has been played
            .Gather(timeout: 10, numDigits: 10);

        var call = CallResource.Create(
            twiml: new Twiml(voice.ToString()),
            from: new PhoneNumber(_numbersConfig.DefaultSender),
            to: new PhoneNumber(phoneNumber),
            record: true
        );

        return Ok(call.Sid);
    }

    [HttpPost]
    [Route(READ_NUMBERS_CALL)]
    public IActionResult ReadNumbersCall(string numbersToSay, string phoneNumber = null)
    {
        if (phoneNumber is null) phoneNumber = _numbersConfig.BGNikola;

        var response = new VoiceResponse()
            .Say("Your numbers are");

        // Sanitizes the user input, gets the numbers as a list
        // and then appends a Play resource for each number
        numbersToSay.GetNumbersOnly()
            .ForEach(n => response.Append(new Play(Number(n))));

        var call = CallResource.Create(
            twiml: new Twiml(response.ToString()),
            from: new PhoneNumber(_numbersConfig.DefaultSender),
            to: new PhoneNumber(phoneNumber),
            record: true
        );

        return Ok(call.Sid);
    }

    [HttpPost]
    [Route(ROUTING_CALL)]
    public IActionResult OutgoingCallWithRouting(string phoneNumber = null)
    {
        if (phoneNumber is null) phoneNumber = _numbersConfig.BGNikola;

        var voice = new VoiceResponse()
            .Say("Hello!")
            .Dial(_numbersConfig.BGNikola);

        var call = CallResource.Create(
            twiml: new Twiml(voice.ToString()),
            from: new PhoneNumber(_numbersConfig.DefaultSender),
            to: new PhoneNumber(phoneNumber),
            record: true
        );

        return Ok(call.Sid);
    }
}
