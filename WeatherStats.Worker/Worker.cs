using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherStats.Kent;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using WeatherStats.Shared.Data;

namespace WeatherStats.Worker
{
    public class WeatherWorker : BackgroundService
    {
        private readonly ILogger<WeatherWorker> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherWorker(ILogger<WeatherWorker> logger,
            IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("create channel");
            using (var channel = GrpcChannel.ForAddress("https://localhost:5001"))
            {
                _logger.LogInformation("channel created");

                _logger.LogInformation("create client");
                var client = new Weather.WeatherClient(channel);
                _logger.LogInformation("client created");

                var d = new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var i = 0;
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation("load weather data");
                        var request = new WeatherRequest
                        {
                            Date = Timestamp.FromDateTime(d)
                        };
                        var weather = await client.GetWeatherAsync(
                            request, null, null, stoppingToken);
                        _logger.LogInformation(
                            $"Temp: {weather.AvgTemperature}; " +
                            $"Precipitaion: {weather.Precipitaion}");

                        await _weatherService.Create(new WeatherData
                        {
                            Id = i,
                            WeatherStation = "US1WAKG0045",
                            AvgTemperature = weather.AvgTemperature,
                            AvgWindSpeed = weather.AvgWindSpeed,
                            MaxTemperature = weather.MaxTemperature,
                            MinTemperature = weather.MinTemperature,
                            Precipitaion = weather.Precipitaion,
                            Date = weather.Date.ToDateTime()
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }
                    d = d.AddDays(1);
                    i++;
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}
