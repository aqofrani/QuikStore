using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weather.Models
{
    /// <summary>
    /// Represent weather api data.
    /// </summary>
    public class WeatherInformation
    {
        public string Style { get; set; }
        public string Description { get; set; }
        public string CityName { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public double MinimumTemperature { get; set; }
        public double MaximumTemperature { get; set; }

        public WeatherInformation()
        {

        }
        public WeatherInformation(JObject data)
        {
            this.Style = data["weather"][0]["main"].ToString();
            this.Description = data["weather"][0]["description"].ToString();
            this.Temperature = Double.Parse(data["main"]["temp"].ToString());
            this.Pressure = Double.Parse(data["main"]["pressure"].ToString());
            this.Humidity = Double.Parse(data["main"]["humidity"].ToString());
            this.MinimumTemperature = Double.Parse(data["main"]["temp_min"].ToString());
            this.MaximumTemperature = Double.Parse(data["main"]["temp_max"].ToString());
            this.CityName = data["name"].ToString();
        }
    }
}