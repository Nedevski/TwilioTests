using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using Twilio.TwiML.Voice;

using TwilioTests.API.Configuration;
using TwilioTests.API.Controllers.Base;
using TwilioTests.API.Helpers;

namespace TwilioTests.API.Controllers;

public class IncomingCallController : TwilioController
{
    private const string START = "Start";
    private const string START_MENU = "Start/Menu";
    private const string ORDER_NUMBER_ENTERED = "Order/NumberEntered";
    private const string ORDER_MESSAGE_RECORDED = "Order/MessageRecorded";

    public IncomingCallController(
        ILogger<IncomingCallController> logger,
        IOptions<TwilioConfiguration> twilioConfiguration) : base(logger, twilioConfiguration)
    {
    }

    [HttpPost]
    [Route(START)]
    public IActionResult Start([FromForm] VoiceRequest request)
    {
        // TwiML classes can be created as standalone elements
        var gather = Gather(1, START_MENU)
            .Say(@"
                To speak to a real monkey, press 1.
                Press 2 to record your own monkey howl.
                To check order status, press 3.
                Press any other key to start over.
            ");

        // Attributes can be set directly on the object
        gather.Timeout = 100;
        gather.MaxSpeechTime = 200;

        var response = new VoiceResponse()
            //.Say("Great success!")
            //.Play(new Uri("http://demo.twilio.com/hellomonkey/monkey.mp3"))
            .Append(gather);

        return response.AsTwiML();
    }

    [HttpPost]
    [Route(START_MENU)]
    public IActionResult MainMenu([FromForm] VoiceRequest request)
    {
        var response = new VoiceResponse();

        // If the user entered digits, process their request
        if (!string.IsNullOrEmpty(request.Digits))
        {
            switch (request.Digits)
            {
                case "1":
                    response.Say("Ooga booga!");
                    break;
                case "2":
                    response
                        .Say("Return to monkey!")
                        .Play(new Uri("http://demo.twilio.com/hellomonkey/monkey.mp3"));
                    break;
                case "3":
                    var gatherOrder = new Gather(numDigits: 5, action: Action(ORDER_NUMBER_ENTERED))
                        .Say("Please enter order number");

                    response.Append(gatherOrder);
                    break;
                default:
                    response.Say("Sorry, I don't understand that choice.").Pause();
                    response.Redirect(Action(START));
                    break;
            }
        }
        else
        {
            // If no input was sent, redirect to the /voice route
            response
                .Say("Invalid input")
                .Redirect(Action(START));
        }

        return response.AsTwiML();
    }

    [HttpPost]
    [Route(ORDER_NUMBER_ENTERED)]
    public IActionResult OrderNumberEntered([FromForm] VoiceRequest request)
    {
        var response = new VoiceResponse();

        // If the user entered digits, process their request
        if (!string.IsNullOrEmpty(request.Digits))
        {
            var digitsSpaced = string.Join(" ", request.Digits.ToArray());

            response
                .Say($"Your order number is {digitsSpaced}")
                .Record(
                    playBeep: true,
                    action: Action(ORDER_MESSAGE_RECORDED)
                );
        }
        else
        {
            // If no input was sent, redirect to the /voice route
            response
                .Say("Invalid input. Goodbye")
                .Hangup();
        }

        return response.AsTwiML();
    }

    [HttpPost]
    [Route(ORDER_MESSAGE_RECORDED)]
    public IActionResult OrderRecording([FromForm] VoiceRequest request)
    {
        var response = new VoiceResponse();

        string statusMsg = "Fail";

        if (!string.IsNullOrEmpty(request.RecordingUrl))
        {
            statusMsg = "Success";

            _logger.LogError(JsonConvert.SerializeObject(request));
        }

        response
            .Say($"Recording {statusMsg}")
            .Hangup();

        return response.AsTwiML();
    }
}
