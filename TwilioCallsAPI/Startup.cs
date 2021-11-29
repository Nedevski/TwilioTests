using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

using TwilioTests.API.Configuration;

namespace TwilioTests.API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<TwilioConfiguration>(Configuration.GetSection(nameof(TwilioConfiguration)));
        services.Configure<NumbersConfiguration>(Configuration.GetSection(nameof(NumbersConfiguration)));

        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Twilio Call API", Version = "v1" });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Twilio Call API v1"));

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseStaticFiles();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
