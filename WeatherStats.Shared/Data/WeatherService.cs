using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace WeatherStats.Shared.Data
{
    public interface IWeatherService
    {
        Task<List<WeatherData>> Get();
        Task<WeatherData> Get(int id);
        Task<WeatherData> Create(WeatherData weather);
        Task Update(int id, WeatherData weatherIn);
        Task Remove(WeatherData weatherIn);
        Task Remove(int id);
    }
    public class WeatherService : IWeatherService
    {
        private readonly IMongoCollection<WeatherData> _weatherData;

        public WeatherService(IWeatherDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _weatherData = database.GetCollection<WeatherData>(
                settings.WeatherCollectionName);
        }

        public async Task<List<WeatherData>> Get() =>
            (await _weatherData.FindAsync(book => true)).ToList();

        public async Task<WeatherData> Get(int id) =>
            (await _weatherData.FindAsync<WeatherData>(weather => weather.Id == id)).FirstOrDefault();

        public async Task<WeatherData> Create(WeatherData weather)
        {
            await _weatherData.InsertOneAsync(weather);
            return weather;
        }

        public async Task Update(int id, WeatherData weatherIn) =>
            await _weatherData.ReplaceOneAsync(weather => weather.Id == id, weatherIn);

        public async Task Remove(WeatherData weatherIn) =>
            await _weatherData.DeleteOneAsync(weather => weather.Id == weatherIn.Id);

        public async Task Remove(int id) =>
            await _weatherData.DeleteOneAsync(weather => weather.Id == id);
    }
}