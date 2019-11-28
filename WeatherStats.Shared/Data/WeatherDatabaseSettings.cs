namespace WeatherStats.Shared.Data
{
    public class WeatherDatabaseSettings : IWeatherDatabaseSettings
    {
        public string WeatherCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IWeatherDatabaseSettings
    {
        string WeatherCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}