using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using weatherAssistant.Helpers.Interfaces;
using weatherAssistant.Models;

namespace weatherAssistant.Services
{
    /// <summary>
    /// Class that helps in managing queries for OpenWeatherMap API with his limitations
    /// </summary>
    public sealed class WeatherService
    {
        private IOWMQueryBuilder _queryBuilder;
        private IJsonDownloader _jsonDownloader;

        public WeatherService(IOWMQueryBuilder queryBuilder, IJsonDownloader jsonDownloader)
        {
            _queryBuilder = queryBuilder;
            _jsonDownloader = jsonDownloader;
        }


        public async Task<CurrentWeather.root> getCurrentWeatherByCityName(string city)
        {
            return await Task.Run(() =>
            {
                string url = _queryBuilder.CurrentWeatherByCityName(city);

                var OutPut = _jsonDownloader.Download<CurrentWeather.root>(url);
                return OutPut;
            });

        }
        public async Task<WeatherForecast.root> getWeatherForecastByCityName(string city)
        {
            return await Task.Run(() =>
            {
                string url = _queryBuilder.ForecastWeatherByCityName(city);

                var OutPut = _jsonDownloader.Download<WeatherForecast.root>(url);
                return OutPut;
            });
        }

        public async Task<CurrentWeather.root> getCurrentWeatherByCoordinate(double longitude, double latitude)
        {
            return await Task.Run(() =>
            {
                string url = _queryBuilder.CurrentWeatherByCoordinate(longitude, latitude);

                var OutPut = _jsonDownloader.Download<CurrentWeather.root>(url);
                return OutPut;
            });

        }
    }
}
