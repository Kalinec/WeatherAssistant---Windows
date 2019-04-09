using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAssistant___TaskMonitor.StormAPI;

namespace WeatherAssistant___TaskMonitor
{
    public sealed class StormService
    {
        //singleton implementation
        private static StormService _instance = null;
        private static readonly object _lock = new object();

        public static StormService Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new StormService();
                    }

                    return _instance;
                }
            }
        }

        private serwerSOAPPortClient _stormAPI = new serwerSOAPPortClient();
        private const string API_ID = "3f04fbcac562e34c59d03cc166dc532a9451ded3";

        public MyComplexTypeBurza GetStormInfo(int radius, double latitude, double longitude)
        {
            return _stormAPI.szukaj_burzy(latitude.ToString(), longitude.ToString(), radius,
                API_ID);
        }

        public MyComplexTypeOstrzezenia GetWeatherWarnings(double latitude, double longitude)
        {
            return _stormAPI.ostrzezenia_pogodowe((float)latitude, (float)longitude, API_ID);
        }

    }
}
