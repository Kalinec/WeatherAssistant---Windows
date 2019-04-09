using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using weatherAssistant.Helpers;
using weatherAssistant.Helpers.Interfaces;
using weatherAssistant.Models;
using weatherAssistant.Helpers.Converters;
using weatherAssistant.ViewModels;
using weatherAssistant.ViewModels.Interfaces;
using Windows.UI.Xaml;
using System.Globalization;

namespace WeatherAssistant.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_download_current_weather_json()
        {
            //prepare
            IJsonDownloader downloader = new JsonDownloader();
            Uri uri = new Uri(Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())), "currentWeather_response.txt"));

            //do it
            var response = downloader.Download<CurrentWeather.root>(uri.AbsoluteUri);

            //asserts
            Assert.AreEqual(response.visibility, 10000);
            Assert.AreEqual(response.wind.speed, 4.1);
            Assert.AreEqual(response.weather[0].main, "Drizzle");
        }

        [TestMethod]
        public void Can_convert_from_bool_to_visibility()
        {
            //prepare
            var boolToVisibilityOrHidden = new BoolToVisibleOrHidden();

            //do it :)
            var convertResult = boolToVisibilityOrHidden.Convert((object)true, typeof(bool), null, CultureInfo.CurrentCulture);
            var convertBackResult =
                boolToVisibilityOrHidden.ConvertBack((object)convertResult, typeof(Visibility), null, CultureInfo.CurrentCulture);

            //asserts
            Assert.AreEqual(0, (int)Enum.Parse(typeof(Visibility), convertResult.ToString()));
        }

        [TestMethod]
        public void Can_convert_from_degress_to_direction()
        {
            //prepare
            var degressToDirection = new DegressToDirectionConverter();

            string expectedEast;
            string expectedWest;
            if (CultureInfo.CurrentCulture.Name == "pl-PL")
            {
                expectedEast = "Wschodni";
                expectedWest = "Zachodni";
            }
            else
            {
                expectedEast = "East";
                expectedWest = "West";
            }

            //do it :)
            var convertEastResult = degressToDirection.Convert((object)90.00, typeof(double), null, CultureInfo.CurrentCulture);
            var convertWestResult =
                degressToDirection.Convert((object)270.00, typeof(double), null, CultureInfo.CurrentCulture);

            //asserts
            Assert.AreEqual(expectedEast, convertEastResult.ToString());
            Assert.AreEqual(expectedWest, convertWestResult.ToString());
        }

        [TestMethod]
        public void Can_convert_from_english_to_polish_condition()
        {
            //prepare
            var englishToPolishCondition = new EnglishToPolishConditionConverter();
            string expectedCloudsValue;

            if (CultureInfo.CurrentCulture.Name == "pl-PL")
                expectedCloudsValue = "Chmury";
            else
                expectedCloudsValue = "Clouds";

            //do it :)
            var convertResult =
                englishToPolishCondition.Convert((object)"Clouds", typeof(string), null, CultureInfo.CurrentCulture);

            //asserts
            Assert.AreEqual(expectedCloudsValue, convertResult.ToString());
        }

        [TestMethod]
        public void Can_change_view_model()
        {
            //prepare
            MainWindowViewModel mainViewModel = new MainWindowViewModel();
            IPageViewModel weatherForecastVM = new WeatherForecastViewModel();
            //do it :) 
            mainViewModel.ChangePageCommand.Execute(weatherForecastVM);
            //asserts
            Assert.AreEqual(weatherForecastVM, mainViewModel.CurrentPageViewModel);
        }

        [TestMethod]
        public void Can_build_mapbox_query()
        {
            //prepare
            IMapBoxQueryBuilder mapBoxQueryBuilder = new MapBoxQueryBuilder();
            //do it :)
            string forwardGeocoderResult = mapBoxQueryBuilder.ForwardGeocoder("Lublin");
            string directionResult = mapBoxQueryBuilder.Direction("walking", 23.00, 23.01, 23.02, 23.03);
            //asserts
            Assert.AreEqual("https://api.mapbox.com/geocoding/v5/mapbox.places/Lublin.json?access_token=pk.eyJ1Ijoia2FsaW5lYyIsImEiOiJjam1taWhiZDUwaHhnM2twYm4wZzV5cHFjIn0.NTnV11b82zHlFdXU998L7Q", forwardGeocoderResult);
            Assert.AreEqual("https://api.mapbox.com/directions/v5/mapbox/walking/23,23.01;23.02,23.03?overview=full&annotations=distance&geometries=geojson&access_token=pk.eyJ1Ijoia2FsaW5lYyIsImEiOiJjam1taWhiZDUwaHhnM2twYm4wZzV5cHFjIn0.NTnV11b82zHlFdXU998L7Q", directionResult);
        }

        [TestMethod]
        public void Can_build_owm_query()
        {
            //prepare
            IOWMQueryBuilder owmQueryBuilder = new OWMQueryBuilder();

            //do it :)
            string currentWeatherByCityNameResult = owmQueryBuilder.CurrentWeatherByCityName("Lublin");
            string currentWeatherByCoordinateResult = owmQueryBuilder.CurrentWeatherByCoordinate(23.00, 23.01);
            string forecastWeatherByCityNameResult = owmQueryBuilder.ForecastWeatherByCityName("Lublin");

            //asserts
            Assert.AreEqual("http://api.openweathermap.org/data/2.5/weather?q=Lublin&appid=ac545ebcce565f9c91956383c030f848&units=metric&lang=pl", currentWeatherByCityNameResult);
            Assert.AreEqual("http://api.openweathermap.org/data/2.5/weather?lat=23,01&lon=23&appid=ac545ebcce565f9c91956383c030f848&units=metric&lang=pl", currentWeatherByCoordinateResult);
            Assert.AreEqual("http://api.openweathermap.org/data/2.5/forecast?q=Lublin&appid=ac545ebcce565f9c91956383c030f848&units=metric&lang=pl", forecastWeatherByCityNameResult);
        }
    }
}
