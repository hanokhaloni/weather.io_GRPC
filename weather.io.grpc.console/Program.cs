using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace weather.io.grpc.console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Weather.WeatherClient(channel);
            var reply = await client.GetWeatherAsync(
                              new WeatherRequest { Name = "GreeterClient" });
            Console.WriteLine("Weather: " + reply.Greet);
            foreach (var forecast in reply.Items)
            {
                Console.WriteLine("On {0} it will be {1}C / {2}F - {3}"
                    , forecast.Date
                    , forecast.TemperatureC
                    , forecast.TemperatureF
                    , forecast.Summary);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static async Task Greet(string[] args)
        {
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(
                              new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

    }
}
