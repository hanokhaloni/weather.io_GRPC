using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace weather.io.grpc.Services
{
    public class WeatherService : Weather.WeatherBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<GreeterService> _logger;
        public WeatherService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<WeatherReply> GetWeather(WeatherRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GetWeather to {0}", request.Name);
            var weatherReply = new WeatherReply();

            var rng = new Random();
            weatherReply.Items.AddRange(Enumerable.Range(1, 5).Select(index =>
            {
                var randomTemperatureC = rng.Next(-20, 55);
                return new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index).ToShortDateString(),
                    TemperatureC = randomTemperatureC,
                    TemperatureF = 32 + (int)(randomTemperatureC / 0.5556),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                };
            })

            .ToArray());


            return Task.FromResult(weatherReply);

        }
    }
}
