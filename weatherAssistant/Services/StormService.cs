using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weatherAssistant.StormApi;

namespace weatherAssistant.Services
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
        //

        // Latitude N  Longitude E

        private serwerSOAPPortClient _stormApi = new serwerSOAPPortClient();

        public MyComplexTypeMiejscowosc GetLocationByCityName(string city)
        {
            return _stormApi.miejscowosc(city, Properties.Settings.Default.Storm_APPID);
        }

        public string GetListLocationByCityName(string city)
        {
            return _stormApi.miejscowosci_lista(city, null, Properties.Settings.Default.Storm_APPID);
        }

        public MyComplexTypeBurza GetStormInfo(int radius, MyComplexTypeMiejscowosc locationInfo)
        {
            return _stormApi.szukaj_burzy(locationInfo.y.ToString(), locationInfo.x.ToString(), radius,
                Properties.Settings.Default.Storm_APPID);
        }

        public MyComplexTypeOstrzezenia GetWeatherWarnings(MyComplexTypeMiejscowosc locationInfo)
        {
            return _stormApi.ostrzezenia_pogodowe((float)locationInfo.y, (float)locationInfo.x, Properties.Settings.Default.Storm_APPID);
        }
    }
}
