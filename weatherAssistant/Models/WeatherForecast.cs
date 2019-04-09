using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weatherAssistant.Helpers.Interfaces;

namespace weatherAssistant.Models
{

    public class WeatherForecast: IOpenWeatherMapInfo
    {


        public class Main
        {
            public double temp { get; set; }
            public double temp_min { get; set; }
            public double temp_max { get; set; }
            public double pressure { get; set; }
            public double sea_level { get; set; }
            public double grnd_level { get; set; }
            public int humidity { get; set; }
            public double temp_kf { get; set; }
        }

        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

        public class Clouds
        {
            public int all { get; set; }
        }

        public class Wind
        {
            public double speed { get; set; }
            public double deg { get; set; }
        }

        public class Rain
        {
            public double __invalid_name__3h { get; set; }
        }

        public class Sys
        {
            public string pod { get; set; }
        }

        public class List
        {
            public int dt { get; set; }
            public Main main { get; set; }
            public List<Weather> weather { get; set; }
            public Clouds clouds { get; set; }
            public Wind wind { get; set; }
            public Rain rain { get; set; }
            public Sys sys { get; set; }
            public string dt_txt { get; set; }
        }

        public class Coord
        {
            public double lat { get; set; }
            public double lon { get; set; }
        }

        public class City
        {
            public int id { get; set; }
            public string name { get; set; }
            public Coord coord { get; set; }
            public string country { get; set; }
            public int population { get; set; }
        }

        public class root
        {
            public string cod { get; set; }
            public double message { get; set; }
            public int cnt { get; set; }
            public List<List> list { get; set; }
            public City city { get; set; }
        }

        /*
        public city city { get; set; }
        public List<list> ForecastList { get; set; }

    }

    public class temp
    {
        public double day { get; set; }
    }

    public class weather
    {
        public string main { get; set; } //condition
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class city
    {
        public string name { get; set; }
    }
    public class list
    {
        public double dt { get; set; } //day in milliseconds
        public double pressure { get; set; } //pressure hpa
        public double humidity { get; set; } //humidity %
        public double speed { get; set; } // wind speed km/h
        public temp temp { get; set; }
        public List<weather> weather { get; set; }
    }
    */
    }
}
