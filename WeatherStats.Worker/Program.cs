using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WeatherStats.Shared.Data;

namespace WeatherStats.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<WeatherDatabaseSettings>(
                        hostContext.Configuration.GetSection(nameof(WeatherDatabaseSettings)));

                    services.AddSingleton<IWeatherDatabaseSettings>(sp =>
                        sp.GetRequiredService<IOptions<WeatherDatabaseSettings>>().Value);

                    services.AddTransient<IWeatherService, WeatherService>();

                    services.AddHostedService<WeatherWorker>();
                });
    }
}
