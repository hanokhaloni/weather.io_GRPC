using System;
using System.Threading.Tasks;
using Grpc.Net.Client;

using System.Configuration;
using McMaster.Extensions.CommandLineUtils;

namespace weather.io.grpc.console
{
    [Command(Name = "The application name", Description = "The application description")]
    [HelpOption("-?")]
    class Program
    {
        [Option("-s|--server", Description = "Sets server base url")]
        private string ServerBaseUrl { get; set; }

        [Option("-g|--greet", Description = "Calls additional greet request")]
        private bool ShouldGreet { get; set; }

        static void Main(string[] args)
        {
            CommandLineApplication.Execute<Program>(args);
        }

        private void OnExecute()
        {
            if (String.IsNullOrEmpty(ServerBaseUrl))
            {
                throw new ArgumentException("Strange - no ServerBaseUrl found. Exiting...");
            }

            if (ShouldGreet)
            {
                Greet(ServerBaseUrl);
            }

            GetWeatherForecast(ServerBaseUrl);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private void GetWeatherForecast(string serverBaseUrl)
        {
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress(serverBaseUrl);
            var client = new Weather.WeatherClient(channel);
            var reply =  client.GetWeather(new WeatherRequest { Name = "GreeterClient" });

            Console.WriteLine("Weather: " + reply.Greet);
            foreach (var forecast in reply.Items)
            {
                Console.WriteLine("On {0} it will be {1}C / {2}F - {3}", forecast.Date, forecast.TemperatureC,
                    forecast.TemperatureF, forecast.Summary);
            }
        }

        private void Greet(string serverBaseUrl)
        {
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress(serverBaseUrl);
            var client = new Greeter.GreeterClient(channel);
            var reply = client.SayHello(
                              new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);
        }

    }
}
