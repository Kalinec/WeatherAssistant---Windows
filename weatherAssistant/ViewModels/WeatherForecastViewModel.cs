using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using weatherAssistant.Commands;
using weatherAssistant.Helpers;
using weatherAssistant.Models;
using weatherAssistant.Services;
using weatherAssistant.ViewModels.Interfaces;

namespace weatherAssistant.ViewModels
{
    public class WeatherForecastViewModel : BaseViewModel, IPageViewModel
    {
        #region Private fields

        private string _city;
        private string _country;
        private string _longitude;
        private string _latitude;
        private string _weatherIcon;
        private string _temperature;
        private string _condition;
        private string _description;
        private string _humidity;
        private string _cloudiness;
        private string _windSpeed;
        private double _windDirection;
        private string _pressure;
        private long _sunrise;
        private long _sunset;
        private bool   _allowToDisplay;


        private CurrentWeather.root currentWeatherDataFromApi;
        private WeatherForecast.root forecastWeatherDataFromApi;

        #endregion

        #region Private dependencies

        private WeatherService OWMmanager;
        #endregion

        #region Public properties

        public string Name
        {
            get { return Properties.Resources.Menu_WeatherForecast; }
        }

        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }

        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                OnPropertyChanged();
            }
        }

        public string Longitude
        {
            get { return _longitude; }
            set
            {
                _longitude = value;
                OnPropertyChanged();
            }
        }

        public string Latitude
        {
            get { return _latitude; }
            set
            {
                _latitude = value;
                OnPropertyChanged();
            }
        }


        public string WeatherIcon
        {
            get { return _weatherIcon; }
            set
            {
                _weatherIcon = value;
                OnPropertyChanged();
            }
        }

        public string Temperature
        {
            get { return _temperature; }
            set
            {
                _temperature = value;
                OnPropertyChanged();
            }
        }

        public string Condition
        {
            get { return _condition; }
            set
            {
                _condition = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public string Humidity
        {
            get { return _humidity; }
            set
            {
                _humidity = value;
                OnPropertyChanged();
            }
        }

        public string Cloudiness
        {
            get { return _cloudiness; }
            set
            {
                _cloudiness = value;
                OnPropertyChanged();
            }
        }

        public string WindSpeed
        {
            get { return _windSpeed; }
            set
            {
                _windSpeed = value;
                OnPropertyChanged();
            }
        }

        public double WindDirection
        {
            get { return _windDirection; }
            set
            {
                _windDirection = value;
                OnPropertyChanged();
            }
        }

        public string Pressure
        {
            get { return _pressure; }
            set
            {
                _pressure = value;
                OnPropertyChanged();
            }
        }

        public long Sunrise
        {
            get { return _sunrise; }
            set
            {
                _sunrise = value;
                OnPropertyChanged();
            }
        }

        public long Sunset
        {
            get { return _sunset; }
            set
            {
                _sunset = value;
                OnPropertyChanged();
            }
        }

        //Allow to display information about the Weather only if the data has been downloaded succesfully
        public bool AllowToDisplay
        {
            get { return _allowToDisplay; }
            set
            {
                _allowToDisplay = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<WeatherForecastSimple> WeatherForecastSimple { get; private set; }
        #endregion

        public WeatherForecastViewModel()
        {
            //Karol tu trzeba kurna zmienić na IoC wiesz?
            OWMmanager = new WeatherService(new OWMQueryBuilder(), new JsonDownloader());
            GetWeatherOwiApi(Properties.Settings.Default.OWM_cityName);
            AllowToDisplay = false;
            WeatherForecastSimple = new ObservableCollection<WeatherForecastSimple>();
            GetForecastOwiApi("Warsaw");

        }

        #region Command properties

        public ICommand GetWeatherAndForecastCommand
        {
            get { return new BaseCommand(GetWeatherAndForecast); }
        }
        public ICommand GetWeatherOwiApiCommand
        {
            get { return new BaseCommand(GetWeatherOwiApi);}
        }

        #endregion

        #region Methods

        public void GetWeatherAndForecast(object city)
        {
            if (city == "")
            {
                MessageBox.Show("City jest nullem");
                return;
            }

            GetWeatherOwiApi(city);
            GetForecastOwiApi(city);
        }

        public async void GetWeatherOwiApi(object City)
        {
            currentWeatherDataFromApi = await OWMmanager.getCurrentWeatherByCityName(City.ToString());

            if (currentWeatherDataFromApi != null)
            {
                UpdateWeatherProperty(currentWeatherDataFromApi);

                AllowToDisplay = true;
                Properties.Settings.Default.OWM_cityName = City.ToString();
            }

            else
                AllowToDisplay = false;

        }

        public async void GetForecastOwiApi(object City)
        {
            forecastWeatherDataFromApi = await OWMmanager.getWeatherForecastByCityName(City.ToString());
            
            if (forecastWeatherDataFromApi != null)
            {
                WeatherForecastSimple.Clear();

                for (int i = 0; i < forecastWeatherDataFromApi.cnt; i++)
                {
                    WeatherForecastSimple.Add(new WeatherForecastSimple(
                        forecastWeatherDataFromApi.list[i].weather[0].main,
                        forecastWeatherDataFromApi.list[i].weather[0].description,
                        Math.Round(forecastWeatherDataFromApi.list[i].wind.speed * 3.6, 0),
                        Math.Round(forecastWeatherDataFromApi.list[i].main.temp, 0),
                        forecastWeatherDataFromApi.list[i].dt,
                        forecastWeatherDataFromApi.list[i].weather[0].icon)); 
                } 
            }
        }

        private void UpdateWeatherProperty(CurrentWeather.root data)
        {
            City = currentWeatherDataFromApi.name;
            Country = currentWeatherDataFromApi.sys.country;
            Longitude = currentWeatherDataFromApi.coord.lon.ToString();
            Latitude = currentWeatherDataFromApi.coord.lat.ToString();
            Temperature = ((int)currentWeatherDataFromApi.main.temp).ToString();
            Condition = currentWeatherDataFromApi.weather[0].main;
            Description = currentWeatherDataFromApi.weather[0].description;
            Humidity = currentWeatherDataFromApi.main.humidity.ToString();
            Cloudiness = currentWeatherDataFromApi.clouds.all.ToString();
            WindSpeed = Math.Round(currentWeatherDataFromApi.wind.speed * 3.6, 0).ToString();
            WindDirection = currentWeatherDataFromApi.wind.deg;
            Pressure = currentWeatherDataFromApi.main.pressure.ToString();
            Sunrise = (long)currentWeatherDataFromApi.sys.sunrise;
            Sunset = (long)currentWeatherDataFromApi.sys.sunset;
            WeatherIcon = currentWeatherDataFromApi.weather[0].icon;
            
        }
        #endregion
    }
}
