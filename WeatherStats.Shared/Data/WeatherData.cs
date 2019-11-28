using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeatherStats.Shared.Data
{
    public class WeatherData
    {
        [BsonId]
        public int Id { get; set; }
        public string WeatherStation { get; set; }
        public DateTime Date { get; set; }

        public float? AvgTemperature { get; set; }
        public float? MinTemperature { get; set; }
        public float? MaxTemperature { get; set; }

        public float? AvgWindSpeed { get; set; }

        public float? Precipitaion { get; set; }
    }
}