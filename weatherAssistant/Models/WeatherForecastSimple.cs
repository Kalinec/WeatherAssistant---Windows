using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weatherAssistant.Models
{
    public class WeatherForecastSimple
    {
        public string Condition { get; set; }
        public string Description { get; set; }
        public double WindSpeed { get; set; }
        public double Temperature { get; set; }
        public long DateTime { get; set; } //milliseconds
        public string Icon { get; set; }

        public WeatherForecastSimple(string condition, string description, double windSpeed, double temperature,
            long dateTime, string icon)
        {
            this.Condition = condition;
            this.Description = description;
            this.WindSpeed = windSpeed;
            this.Temperature = temperature;
            this.DateTime = dateTime;
            this.Icon = icon;
        }
    }
}
