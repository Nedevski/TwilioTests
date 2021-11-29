using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.ComponentModel.DataAnnotations;

using TwilioTests.API.Configuration;
using TwilioTests.API.Controllers.Base;

namespace TwilioTests.API.Controllers;

public class SmsController : TwilioController
{
    private readonly NumbersConfiguration _numbersConfig;

    public SmsController(
        ILogger<SmsController> logger,
        IOptions<TwilioConfiguration> twilioConfiguration,
        IOptions<NumbersConfiguration> numbersConfiguration) : base(logger, twilioConfiguration)
    {
        _numbersConfig = numbersConfiguration.Value;
    }

    public record SendSmsRequest([Required] string Body, string Receiver);

    [HttpPost]
    [Route("Send")]
    public IActionResult Send([FromBody] SendSmsRequest request)
    {
        var receiver = request.Receiver.ToLower() == "ivet"
            ? new PhoneNumber(_numbersConfig.BGIvet)
            : new PhoneNumber(_numbersConfig.BGNikola);

        var message = MessageResource.Create(
            body: request.Body,
            from: new PhoneNumber(_numbersConfig.DefaultSender),
            to: receiver
        );

        return Ok(message.Sid);
    }
}
