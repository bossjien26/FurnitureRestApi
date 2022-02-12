using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace RestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.SetMinimumLevel(LogLevel.Trace);
                    logging.ClearProviders(); // removes all providers from LoggerFactory
                    logging.AddDebug();
                    logging.AddConsole();
                    logging.AddTraceSource("Information, ActivityTracing"); // Add Trace listener provider
                    logging.AddNLog($"nlog.{hostContext.HostingEnvironment.EnvironmentName}.config");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://+:5000");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
