using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using WeatherStats.Kent.Data;
using Google.Protobuf.WellKnownTypes;

namespace WeatherStats.Kent
{
    public class WeatherService : Weather.WeatherBase
    {
        private readonly ILogger<WeatherService> _logger;
        private readonly ApplicationDbContext _dbContext;

        public WeatherService(
            ILogger<WeatherService> logger,
            ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public override Task<WeatherReply> GetWeather(
            WeatherRequest request,
            ServerCallContext context)
        {
            var weatherData = _dbContext.WeatherData
                .SingleOrDefault(x => x.WeatherStationId == WeatherStations.Kent
                    && x.Date == request.Date.ToDateTime());

            if (weatherData is null)
            {
                return Task.FromResult(new WeatherReply());
            }
            return Task.FromResult(new WeatherReply
            {
                Date = Timestamp.FromDateTime(weatherData.Date.ToUniversalTime()),
                AvgTemperature = weatherData.AvgTemperature ?? float.MinValue,
                MinTemperature = weatherData.MinTemperature ?? float.MinValue,
                MaxTemperature = weatherData.MaxTemperature ?? float.MinValue,
                AvgWindSpeed = weatherData.AvgWindSpeed ?? float.MinValue,
                Precipitaion = weatherData.Precipitaion ?? float.MinValue
            });
        }
    }
}
