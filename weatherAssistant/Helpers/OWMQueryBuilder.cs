using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using weatherAssistant.Helpers.Interfaces;

namespace weatherAssistant.Helpers
{
    /// <summary>
    /// This class build a query to OpenWeatherMap API
    /// </summary>
    public class OWMQueryBuilder : IOWMQueryBuilder
    {
        public string Url { get; private set; }

        public string CurrentWeatherByCityName([NotNull]string city)
        { 
            Url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units={2}&lang={3}", 
                city,
                Properties.Settings.Default.OWM_APPID, 
                Properties.Settings.Default.OWM_units,
                Properties.Settings.Default.Language);
            return Url;
        }

        public string CurrentWeatherByCoordinate([NotNull] double longitude, [NotNull] double latitude)
        {
            Url = string.Format(
                "http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}&units={3}&lang={4}", 
                latitude,
                longitude,  
                Properties.Settings.Default.OWM_APPID, 
                Properties.Settings.Default.OWM_units,
                Properties.Settings.Default.Language);
            return Url;
        }

        public string ForecastWeatherByCityName([NotNull] string city)
        {
            Url = string.Format("http://api.openweathermap.org/data/2.5/forecast?q={0}&appid={1}&units={2}&lang={3}",
                city,
                Properties.Settings.Default.OWM_APPID, 
                Properties.Settings.Default.OWM_units,
                Properties.Settings.Default.Language);

            return Url;
        }

        public string CitiesInRectangleZone([NotNull] string longitudeLeft, [NotNull] string latitudeBottom,
            [NotNull] string longitudeRight, [NotNull] string latitudeTop, [NotNull] string zoom)
        {
            Url = string.Format("http://api.openweathermap.org/data/2.5/box/city?bbox={0},{1},{2},{3},{4}",
                longitudeLeft, 
                latitudeBottom, 
                longitudeRight, 
                latitudeTop, 
                zoom);
            return Url;
        }
    }
}
