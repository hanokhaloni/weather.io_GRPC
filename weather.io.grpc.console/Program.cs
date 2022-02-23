using System;
using System.Threading.Tasks;
using Grpc.Net.Client;

using System.Configuration;

namespace weather.io.grpc.console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serverBaseUrl = ConfigurationManager.AppSettings.Get("ServerBaseUrl");

            if (String.IsNullOrEmpty(serverBaseUrl))
            {
                throw new ArgumentException("Strange - no ServerBaseUrl found. Exiting...");
            }

            if (args.Length == 1)
            {
                if (args[0].Contains("Greet", StringComparison.OrdinalIgnoreCase))
                {
                    await Greet(serverBaseUrl);
                }
            }
            await GetWeatherForecast(serverBaseUrl);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static async Task GetWeatherForecast(string serverBaseUrl)
        {
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress(serverBaseUrl);
            var client = new Weather.WeatherClient(channel);
            var reply = await client.GetWeatherAsync(new WeatherRequest { Name = "GreeterClient" });

            Console.WriteLine("Weather: " + reply.Greet);
            foreach (var forecast in reply.Items)
            {
                Console.WriteLine("On {0} it will be {1}C / {2}F - {3}", forecast.Date, forecast.TemperatureC,
                    forecast.TemperatureF, forecast.Summary);
            }
        }

        static async Task Greet(string serverBaseUrl)
        {
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress(serverBaseUrl);
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(
                              new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);
        }

    }
}
