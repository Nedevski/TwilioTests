global using Twilio.AspNet.Common;
global using Twilio.Rest.Api.V2010.Account;
global using Twilio.TwiML;
global using Twilio.Types;

namespace TwilioTests.API;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
