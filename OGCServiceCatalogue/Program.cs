using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OGCServiceCatalogue
{
    public class Program
    {
        private readonly ILogger _logger;
        public static void Main(string[] args)
        {
           var logFactory = new LoggerFactory()
         .AddConsole(LogLevel.Warning)
         .AddConsole()
         .AddDebug();
            string GOSTServerAddress = Environment.GetEnvironmentVariable("GOSTServerAddress");
            if (!(GOSTServerAddress == null || GOSTServerAddress == ""))
            {
                settings.GOSTServerAddress = GOSTServerAddress;
            }
            else
            {
                Console.WriteLine("Error: missing GOSTServerAddress environment variable");
                return;
            }
            string GOSTPrefix = Environment.GetEnvironmentVariable("GOSTPrefix");
            if (!(GOSTPrefix == null || GOSTPrefix == ""))
            {
                settings.GOSTPrefix = GOSTPrefix;
            }
            else
            {
                Console.WriteLine("Error: missing GOSTPrefix environment variable");
                return;
            }
            string MQTTServerAddress = Environment.GetEnvironmentVariable("MQTTServerAddress");
            if (!(MQTTServerAddress == null || MQTTServerAddress == ""))
            {
                settings.MQTTServerAddress = MQTTServerAddress;
            }
            else
            {
                Console.WriteLine("Error: missing MQTTServerAddress environment variable");
                return;
            }
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
