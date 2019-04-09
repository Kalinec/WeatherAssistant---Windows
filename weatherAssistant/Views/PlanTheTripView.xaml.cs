using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp;
using CefSharp.Wpf;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using weatherAssistant.Helpers;
using weatherAssistant.Models;
using weatherAssistant.Services;
using weatherAssistant.StormApi;

namespace weatherAssistant.Views
{
    /// <summary>
    /// Interaction logic for PlanTheTripView.xaml
    /// </summary>
    public partial class PlanTheTripView : UserControl
    {
        private MapBoxAPI_Geocoder.Root _geocoderOrigin;
        private MapBoxAPI_Geocoder.Root _geocoderDestination;
        private MapBoxAPI_Directions.Root _directions;
        private MapBoxService _mapBoxService;
        private List<double> latitude;
        private List<double> longitude;
        private WeatherService _weatherService;
        private CurrentWeather.root _currentWeatherInfo;
        private MyComplexTypeMiejscowosc _locationInfo;
        private MyComplexTypeBurza _stormInfo;
        private MyComplexTypeOstrzezenia _warningsInfo;
        

        //fields for Risk Assessment
        private List<List<int>> listOfWeatherConditionsRisk;
        private int _riskAssessment;
        private int? temperatureRiskLevel;
        private double? minTemperature;
        private double? maxTemperature;

        private int? weatherConditionRiskLevel;
        private string weatherConditionDescription;

        private int? cloudinessRiskLevel;
        private double? minCloudiness;
        private double? maxCloudiness;

        private int? windSpeedRiskLevel;
        private double? minWindSpeed;
        private double? maxWindSpeed;

        private int? visibilityRiskLevel;
        private double? minVisibility;
        private double? maxVisibility;

        private int? frostWarningLevel;
        private int? heatWarningLevel;
        private int? windWarningLevel;
        private int? rainWarningLevel;
        private int? stormWarningLevel;
        private int? tornadoWarningLevel;

        public int RiskLevelAssessment
        {
            get { return _riskAssessment; }
            set
            {
                _riskAssessment = value;
                updateRiskAssessment();
            }
        }
        public PlanTheTripView()
        { 
            InitializeComponent();
            latitude = new List<double>();
            longitude = new List<double>();
            _mapBoxService = new MapBoxService(new MapBoxQueryBuilder(), new JsonDownloader());
            _weatherService = new WeatherService(new OWMQueryBuilder(), new JsonDownloader());
            _locationInfo = new MyComplexTypeMiejscowosc();
            _stormInfo = new MyComplexTypeBurza();
            _warningsInfo = new MyComplexTypeOstrzezenia();
        }

        private void browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // the browser control is initialized, now load the html

            browser.LoadHtml(File.ReadAllText("index.html"));
           // browser.ExecuteScriptAsync("initializeMap();");
        }

        private async void TextBoxOrigin_KeyUp(object sender, KeyEventArgs e)
        {
            bool found = false;
            var border = (resultStack_origin.Parent as ScrollViewer).Parent as Border;

            //Download data
            _geocoderOrigin = await _mapBoxService.getForwardGeocoder(textboxOrigin.Text);

            string query = (sender as TextBox).Text;

            if (query.Length == 0)
            {
                // Clear
                resultStack_origin.Children.Clear();
                border.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                border.Visibility = System.Windows.Visibility.Visible;
            }

            // Clear the list
            resultStack_origin.Children.Clear();

            // Add the result
            if (_geocoderOrigin != null)
            {
                foreach (var obj in _geocoderOrigin.features)
                {
                    if (obj.place_name.ToLower().StartsWith(query.ToLower()))
                    {
                        addItemOrigin(obj.place_name, border);
                        found = true;
                    }
                }
            }
            if (!found)
            {
                resultStack_origin.Children.Add(new TextBlock() { Text = "No results found." });
            }
        }

        private void addItemOrigin(string text, Border border)
        {
            TextBlock block = new TextBlock();

            // Add the text
            block.Text = text;

            // A little style...
            block.Margin = new Thickness(2, 3, 2, 3);
            block.Cursor = Cursors.Hand;

            // Mouse events
            block.MouseLeftButtonUp += (sender, e) =>
            {
                string place = (sender as TextBlock).Text;
                textboxOrigin.Text = place;
                border.Visibility = Visibility.Collapsed;
                foreach (var i in _geocoderOrigin.features)
                {
                    if (i.place_name.Equals(place))
                    {
                        SourceLongitude.Text = i.center[0].ToString();
                        SourceLatitude.Text = i.center[1].ToString();
                    }
                }
            };

            block.MouseEnter += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.PeachPuff;
            };

            block.MouseLeave += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.Transparent;
            };

            // Add to the panel
            resultStack_origin.Children.Add(block);
        }

        private async void TextBoxDestination_KeyUp(object sender, KeyEventArgs e)
        {
            bool found = false;
            var border = (resultStack_destination.Parent as ScrollViewer).Parent as Border;

            //download data
            _geocoderDestination = await _mapBoxService.getForwardGeocoder(textboxDestination.Text);


            string query = (sender as TextBox).Text;

            if (query.Length == 0)
            {
                // Clear
                resultStack_destination.Children.Clear();
                border.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                border.Visibility = System.Windows.Visibility.Visible;
            }

            // Clear the list
            resultStack_destination.Children.Clear();

            // Add the result
            if (_geocoderDestination != null)
            {
                foreach (var obj in _geocoderDestination.features)
                {
                    if (obj.place_name.ToLower().StartsWith(query.ToLower()))
                    {
                        // The word starts with this... Autocomplete must work
                        addItemDestination(obj.place_name, border);
                        found = true;
                    }
                }
            }
            
            if (!found)
            {
                resultStack_destination.Children.Add(new TextBlock() { Text = "No results found." });
            }
        }

        private void addItemDestination(string text, Border border)
        {
            TextBlock block = new TextBlock();

            // Add the text
            block.Text = text;

            // A little style...
            block.Margin = new Thickness(2, 3, 2, 3);
            block.Cursor = Cursors.Hand;

            // Mouse events
            block.MouseLeftButtonUp += (sender, e) =>
            {
                string place = (sender as TextBlock).Text;
                textboxDestination.Text = place;
                border.Visibility = System.Windows.Visibility.Collapsed;
                foreach (var i in _geocoderDestination.features)
                {
                    if (i.place_name.Equals(place))
                    {
                        DestinationLongitude.Text = i.center[0].ToString();
                        DestinationLatitude.Text = i.center[1].ToString();
                    }
                }
            };

            block.MouseEnter += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.PeachPuff;
            };

            block.MouseLeave += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.Transparent;
            };

            // Add to the panel
            resultStack_destination.Children.Add(block);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SourceLatitude.Text.Equals("") || SourceLongitude.Text.Equals("") ||
                DestinationLatitude.Text.Equals("") || DestinationLongitude.Text.Equals(""))
            {
                MessageBox.Show("Nie podano miejsc");
                return;
            }

            latitude.Clear();
            longitude.Clear();
            string routeProfile;

            if (RadioButtonWalking.IsChecked == true)
                routeProfile = "walking";
            else
                routeProfile = "cycling";

             _directions = await _mapBoxService.getDirections(routeProfile, _geocoderOrigin.features[0].center[0], _geocoderOrigin.features[0].center[1],
                _geocoderDestination.features[0].center[0], _geocoderDestination.features[0].center[1]);

            displayDistance();
            displayDuration();
            assessTheRiskAsync();

            foreach (var coords in _directions.routes[0].geometry.coordinates)
            {
                latitude.Add(coords[0]);
                longitude.Add(coords[1]);
            }
            /*foreach (var tracepoint in _mapMatching.tracepoints)
            {
               latitude.Add(tracepoint.location[0]);
               longitude.Add(tracepoint.location[1]);
            } */
            browser.ExecuteScriptAsync("getRoute(" + GetLocality(latitude.ToArray()) + ", " + GetLocality(longitude.ToArray()) + ");");
        }

        public string GetLocality(double[] array)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            string jsonobj = jsSerializer.Serialize(array);
            return jsonobj;       
        }

        private void displayDistance()
        {
            if (_directions.routes[0].distance >= 1000)
                distance.Text = Math.Round(_directions.routes[0].distance / 1000, 2).ToString() + "km";
            else
                distance.Text = Math.Round(_directions.routes[0].distance, 0).ToString() + "m";
        }

        private void displayDuration()
        {
            int hour = (int) (_directions.routes[0].duration / 3600);
            int minutes = (int) ((_directions.routes[0].duration / 60) % 60);
            StringBuilder durationToDisplay = new StringBuilder();

            if (hour != 0)
            {
                durationToDisplay.Append(hour);
                durationToDisplay.Append(" ");
                durationToDisplay.Append(Properties.Resources.Hour);
                durationToDisplay.Append(" ");
            }
            durationToDisplay.Append(minutes);
            durationToDisplay.Append(" ");
            durationToDisplay.Append(Properties.Resources.Minutes);
            duration.Text = durationToDisplay.ToString();
            RouteData.Visibility = Visibility.Visible;
        }

        private async Task assessTheRiskAsync()
        {
            var pointsIndex = designateIndexOfPointsToCheck();
            clearRiskAssessment();
            initializeListOfWeatherConditionsRisk();
            await fetchRiskData(pointsIndex);
            displayRisk();
        }

        private int[] designateIndexOfPointsToCheck()
        {
            List<int> pointsIndex = new List<int>();

            if (_directions.routes[0].distance <= 25000)
            {
                pointsIndex.Add(0); 
                pointsIndex.Add(_directions.routes[0].geometry.coordinates.Count-1);
            }

            else
            {
                double distance = 0;
                pointsIndex.Add(0);

                for (int i = 0; i < _directions.routes[0].legs[0].annotation.distance.Count; i++)
                {
                    distance += _directions.routes[0].legs[0].annotation.distance[i];

                    if (distance >= 25000)
                    {
                        distance = 0;
                        pointsIndex.Add(i);
                    }
                }
            }
            return pointsIndex.ToArray();
        }

        private async Task fetchRiskData(int[] pointsToCheck)
        {
            RiskLevelAssessment = 0;
            foreach (var point in pointsToCheck)
            {
                _locationInfo.x = decimalToDM(_directions.routes[0].geometry.coordinates[point][0]);
                _locationInfo.y = decimalToDM(_directions.routes[0].geometry.coordinates[point][1]);

                _stormInfo =  StormService.Instance.GetStormInfo(25, _locationInfo);
                _warningsInfo = StormService.Instance.GetWeatherWarnings(_locationInfo);

                if (!_stormInfo.liczba.Equals(0))
                {
                    ImageStormRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_high.png", UriKind.Relative));
                    TextStormRisk.Text = Properties.Resources.PlanTheTrip_Risk_High_Storms;
                    RiskLevelAssessment += 15;
                }
                else
                {
                    ImageStormRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_not.png", UriKind.Relative));
                    TextStormRisk.Text = Properties.Resources.PlanTheTrip_Risk_None_Storms;
                }

                if (_warningsInfo.traba != 0 && _warningsInfo.traba > tornadoWarningLevel)
                    tornadoWarningLevel = _warningsInfo.traba;
                if (_warningsInfo.burza != 0 && _warningsInfo.burza > stormWarningLevel)
                    stormWarningLevel = _warningsInfo.burza;
                if (_warningsInfo.opad != 0 && _warningsInfo.opad > rainWarningLevel)
                    rainWarningLevel = _warningsInfo.opad;
                if (_warningsInfo.wiatr != 0 && _warningsInfo.wiatr > windWarningLevel)
                    windWarningLevel = _warningsInfo.wiatr;
                if (_warningsInfo.upal != 0 && _warningsInfo.upal > heatWarningLevel)
                    heatWarningLevel = _warningsInfo.upal;
                if (_warningsInfo.mroz != 0 && _warningsInfo.mroz > frostWarningLevel)
                    frostWarningLevel = _warningsInfo.mroz;

                _currentWeatherInfo = await _weatherService.getCurrentWeatherByCoordinate(_directions.routes[0].geometry.coordinates[point][0],
                    _directions.routes[0].geometry.coordinates[point][1]);

                minTemperature = getMinimumValue(minTemperature, _currentWeatherInfo.main.temp);
                maxTemperature = getMaximumValue(maxTemperature, _currentWeatherInfo.main.temp);

                if (!_currentWeatherInfo.clouds.all.Equals(0))
                {
                    minCloudiness = getMinimumValue(minCloudiness, _currentWeatherInfo.clouds.all);
                    maxCloudiness = getMaximumValue(maxCloudiness, _currentWeatherInfo.clouds.all);
                }
                
                minWindSpeed = getMinimumValue(minWindSpeed, _currentWeatherInfo.wind.speed);
                maxWindSpeed = getMaximumValue(maxWindSpeed, _currentWeatherInfo.wind.speed);

                if (!_currentWeatherInfo.visibility.Equals(0))
                {
                    minVisibility = getMinimumValue(minVisibility, _currentWeatherInfo.visibility);
                    maxVisibility = getMaximumValue(maxVisibility, _currentWeatherInfo.visibility);
                }
                int? oldWeatherConditionRiskLevel = weatherConditionRiskLevel;
                weatherConditionRiskLevel = getWorstWeatherConditionLevel(weatherConditionRiskLevel,
                    _currentWeatherInfo.weather[0].id);

                if (oldWeatherConditionRiskLevel != weatherConditionRiskLevel)
                    weatherConditionDescription = _currentWeatherInfo.weather[0].description;


                if (RadioButtonWalking.IsChecked == true)
                {
                    temperatureRiskLevel = getTemperatureRiskLevelForWalking(minTemperature, maxTemperature);
                    cloudinessRiskLevel = getCloudinessRiskLevelForWalkingAndCycling(maxCloudiness);
                    windSpeedRiskLevel = getWindSpeedRiskLevelForWalking(maxWindSpeed);
                    visibilityRiskLevel = getVisibilityRiskLevelForWalking(maxVisibility);
                }

                else
                {
                    temperatureRiskLevel = getTemperatureRiskLevelForCycling(minTemperature, maxTemperature);
                    cloudinessRiskLevel = getCloudinessRiskLevelForWalkingAndCycling(maxCloudiness);
                    windSpeedRiskLevel = getWindSpeedRiskLevelForCycling(maxWindSpeed);
                    visibilityRiskLevel = getVisibilityRiskLevelForCycling(maxVisibility);
                }
            }
        }

        private void clearRiskAssessment()
        {
            temperatureRiskLevel = null;
            minTemperature = null;
            maxTemperature = null;
            weatherConditionRiskLevel = null;
            weatherConditionDescription = null;
            cloudinessRiskLevel = null;
            minCloudiness = null;
            maxCloudiness = null;
            windSpeedRiskLevel = null;
            minWindSpeed = null;
            maxWindSpeed = null;
            visibilityRiskLevel = null;
            minVisibility = null;
            maxVisibility = null;
            frostWarningLevel = 0;
            heatWarningLevel = 0;
            windWarningLevel = 0;
            rainWarningLevel = 0;
            stormWarningLevel = 0;
            tornadoWarningLevel = 0;
        }

        private void displayRisk()
        {
            RiskDetails.Visibility = Visibility.Visible;
            TextTemperatureRisk.Visibility = Visibility.Visible;
            ImageTemperatureRisk.Visibility = Visibility.Visible;
            TextWindSpeedRisk.Visibility = Visibility.Visible;
            ImageWindSpeedRisk.Visibility = Visibility.Visible;
            TextVisibilityRisk.Visibility = Visibility.Visible;
            ImageVisibilityRisk.Visibility = Visibility.Visible;
            TextCloudinessRisk.Visibility = Visibility.Visible;
            ImageCloudinessRisk.Visibility = Visibility.Visible;
            TextWeatherConditionRisk.Visibility = Visibility.Visible;
            ImageWeatherConditionRisk.Visibility = Visibility.Visible;
            checkWarnings();

            switch (temperatureRiskLevel)
            {
                case 0:
                    if (minTemperature.Value == maxTemperature.Value)
                    {
                        TextTemperatureRisk.Text = Properties.Resources.PlanTheTrip_Risk_None_Temperature +
                                                   " " +
                                                   (int) minTemperature.Value +
                                                   "°C";
                    }
                    else
                    {
                        TextTemperatureRisk.Text = Properties.Resources.PlanTheTrip_Risk_None_Temperature +
                                                   " " +
                                                   (int)minTemperature.Value +
                                                   "°C" +
                                                   "-" +
                                                   (int)maxTemperature.Value +
                                                   "°C";
                    }
                    
                    ImageTemperatureRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_not.png", UriKind.Relative));
                    break;
                case 1:
                    if (minTemperature.Value == maxTemperature.Value)
                    {
                        TextTemperatureRisk.Text = Properties.Resources.PlanTheTrip_Risk_Moderate_Temperature +
                                                   " " +
                                                   (int)minTemperature.Value +
                                                   "°C";
                    }
                    else
                    {
                        TextTemperatureRisk.Text = Properties.Resources.PlanTheTrip_Risk_Moderate_Temperature +
                                                   " " +
                                                   (int)minTemperature.Value +
                                                   "-" +
                                                   (int)maxTemperature.Value +
                                                   "°C";
                    }
                    
                    ImageTemperatureRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_moderate.png", UriKind.Relative));
                    RiskLevelAssessment += 5;
                    break;
                case 2:
                    if (minTemperature.Value == maxTemperature.Value)
                    {
                        TextTemperatureRisk.Text = Properties.Resources.PlanTheTrip_Risk_High_Temperature +
                                                   " " +
                                                   (int)minTemperature.Value +                                 
                                                   "°C";
                    }
                    else
                    {
                        TextTemperatureRisk.Text = Properties.Resources.PlanTheTrip_Risk_High_Temperature +
                                                   " " +
                                                   (int)minTemperature.Value +
                                                   "-" +
                                                   (int)maxTemperature.Value +
                                                   "°C";
                    }
                    ImageTemperatureRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_high.png", UriKind.Relative));
                    RiskLevelAssessment += 15;
                    break;
                case null:
                    TextTemperatureRisk.Visibility = Visibility.Collapsed;
                    ImageTemperatureRisk.Visibility = Visibility.Collapsed;
                    break;
            }

            switch (windSpeedRiskLevel)
            {
                case 0:
                    if (minWindSpeed.Value == maxWindSpeed.Value)
                    {
                        TextWindSpeedRisk.Text = Properties.Resources.PlanTheTrip_Risk_None_Wind +
                                                 " " +
                                                 (int)(minWindSpeed.Value * 3.6) +
                                                 "km/h";
                    }
                    else
                    {
                        TextWindSpeedRisk.Text = Properties.Resources.PlanTheTrip_Risk_None_Wind +
                                                 " " +
                                                 (int)(minWindSpeed.Value * 3.6) +
                                                 "-" +
                                                 (int)(maxWindSpeed.Value * 3.6) +
                                                 "km/h";
                    }
                    ImageWindSpeedRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_not.png", UriKind.Relative));
                    break;
                case 1:
                    if (minWindSpeed.Value == maxWindSpeed.Value)
                    {
                        TextWindSpeedRisk.Text = Properties.Resources.PlanTheTrip_Risk_Moderate_Wind +
                                                 " " +
                                                 (int)(minWindSpeed.Value * 3.6) +
                                                 "km/h";
                    }
                    else
                    {
                        TextWindSpeedRisk.Text = Properties.Resources.PlanTheTrip_Risk_Moderate_Wind +
                                                 " " +
                                                 (int)(minWindSpeed.Value * 3.6) +
                                                 "-" +
                                                 (int)(maxWindSpeed.Value * 3.6) +
                                                 "km/h";
                    }
                    
                    ImageWindSpeedRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_moderate.png", UriKind.Relative));
                    RiskLevelAssessment += 5;
                    break;
                case 2:
                    if (minWindSpeed.Value == maxWindSpeed.Value)
                    {
                        TextWindSpeedRisk.Text = Properties.Resources.PlanTheTrip_Risk_High_Wind +
                                                 " " +
                                                 (int)(minWindSpeed.Value * 3.6) +
                                                 "km/h";
                    }
                    else
                    {
                        TextWindSpeedRisk.Text = Properties.Resources.PlanTheTrip_Risk_High_Wind +
                                                 " " +
                                                 (int)(minWindSpeed.Value * 3.6) +
                                                 "-" +
                                                 (int)(maxWindSpeed.Value * 3.6) +
                                                 "km/h";
                    }
                    ImageWindSpeedRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_high.png", UriKind.Relative));
                    RiskLevelAssessment += 15;
                    break;
                case null:
                    TextWindSpeedRisk.Visibility = Visibility.Collapsed;
                    ImageWindSpeedRisk.Visibility = Visibility.Collapsed;
                    break;
            }

            switch (visibilityRiskLevel)
            {
                case 0:
                    if (minVisibility.Value == maxVisibility.Value)
                    {
                        TextVisibilityRisk.Text = Properties.Resources.PlanTheTrip_Risk_None_Visibility +
                                                  " " +
                                                  (int)(minVisibility.Value / 1000) +
                                                  "km";
                    }
                    else
                    {
                        TextVisibilityRisk.Text = Properties.Resources.PlanTheTrip_Risk_None_Visibility +
                                                  " " +
                                                  (int)(minVisibility.Value / 1000) +
                                                  "-" +
                                                  (int)(maxVisibility.Value / 1000) +
                                                  "km";
                    }
                    
                    ImageVisibilityRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_not.png", UriKind.Relative));
                    break;
                case 1:
                    if (minVisibility.Value == maxVisibility.Value)
                    {
                        TextVisibilityRisk.Text = Properties.Resources.PlanTheTrip_Risk_Moderate_Visibility +
                                                  " " +
                                                  (int)(minVisibility.Value / 1000) +
                                                  "km";
                    }
                    else
                    {
                        TextVisibilityRisk.Text = Properties.Resources.PlanTheTrip_Risk_Moderate_Visibility +
                                                  " " +
                                                  (int)(minVisibility.Value / 1000) +
                                                  "-" +
                                                  (int)(maxVisibility.Value / 1000) +
                                                  "km";
                    }
                    
                    ImageVisibilityRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_moderate.png", UriKind.Relative));
                    RiskLevelAssessment += 2;
                    break;
                case 2:
                    if (minVisibility.Value == maxVisibility.Value)
                    {
                        TextVisibilityRisk.Text = Properties.Resources.PlanTheTrip_Risk_High_Visibility +
                                                  " " +
                                                  (int)(minVisibility.Value / 1000) +
                                                  "km";
                    }
                    else
                    {
                        TextVisibilityRisk.Text = Properties.Resources.PlanTheTrip_Risk_High_Visibility +
                                                  " " +
                                                  (int)(minVisibility.Value / 1000) +
                                                  "-" +
                                                  (int)(maxVisibility.Value / 1000) +
                                                  "km";
                    }
                    
                    ImageVisibilityRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_high.png", UriKind.Relative));
                    RiskLevelAssessment += 5;
                    break;

                case null:
                    TextVisibilityRisk.Visibility = Visibility.Collapsed;
                    ImageVisibilityRisk.Visibility = Visibility.Collapsed;
                    break;
            }

            switch (cloudinessRiskLevel)
            {
                case 0:
                    if (minCloudiness.Value == maxCloudiness.Value)
                    {
                        TextCloudinessRisk.Text = Properties.Resources.PlanTheTrip_Risk_None_Cloudiness +
                                                  " " +
                                                  (int)minCloudiness.Value +
                                                  "%";
                    }
                    else
                    {
                        TextCloudinessRisk.Text = Properties.Resources.PlanTheTrip_Risk_None_Cloudiness +
                                                  " " +
                                                  (int)minCloudiness.Value +
                                                  "-" +
                                                  (int)maxCloudiness.Value +
                                                  "%";
                    }
                    
                    ImageCloudinessRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_not.png", UriKind.Relative));
                    break;
                case 1:
                    if (minCloudiness.Value == maxCloudiness.Value)
                    {
                        TextCloudinessRisk.Text = Properties.Resources.PlanTheTrip_Risk_Moderate_Cloudiness +
                                                  " " +
                                                  (int)minCloudiness.Value +
                                                  "%";
                    }
                    else
                    {
                        TextCloudinessRisk.Text = Properties.Resources.PlanTheTrip_Risk_Moderate_Cloudiness +
                                                  " " +
                                                  (int)minCloudiness.Value +
                                                  "-" +
                                                  (int)maxCloudiness.Value +
                                                  "%";
                    }
                    
                    ImageCloudinessRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_moderate.png", UriKind.Relative));
                    RiskLevelAssessment += 1;
                    break;
                case 2:
                    if (minCloudiness.Value == maxCloudiness.Value)
                    {
                        TextCloudinessRisk.Text = Properties.Resources.PlanTheTrip_Risk_High_Cloudiness +
                                                  " " +
                                                  (int)minCloudiness.Value +
                                                  "%";
                    }
                    else
                    {
                        TextCloudinessRisk.Text = Properties.Resources.PlanTheTrip_Risk_High_Cloudiness +
                                                  " " +
                                                  (int)minCloudiness.Value +
                                                  "-" +
                                                  (int)maxCloudiness.Value +
                                                  "%";
                    }
                    
                    ImageCloudinessRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_high.png", UriKind.Relative));
                    RiskLevelAssessment += 2;
                    break;
                case null:
                    TextCloudinessRisk.Visibility = Visibility.Collapsed;
                    ImageCloudinessRisk.Visibility = Visibility.Collapsed;
                    break;
            }

            switch (weatherConditionRiskLevel)
            {
                case 0:
                    TextWeatherConditionRisk.Text = weatherConditionDescription;
                    ImageWeatherConditionRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_not.png", UriKind.Relative));
                    break;
                case 1:
                    TextWeatherConditionRisk.Text = weatherConditionDescription;
                    ImageWeatherConditionRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_moderate.png", UriKind.Relative));
                    RiskLevelAssessment += 5;
                    break;
                case 2:
                    TextWeatherConditionRisk.Text = weatherConditionDescription;
                    ImageWeatherConditionRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_high.png", UriKind.Relative));
                    RiskLevelAssessment += 15;
                    break;
                case null:
                    TextWeatherConditionRisk.Visibility = Visibility.Collapsed;
                    ImageWeatherConditionRisk.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private double? getMinimumValue(double? currentValue, double valueToCheck)
        {
            if (currentValue == null || currentValue > valueToCheck)
                return valueToCheck;
            return currentValue;
        }

        private double? getMaximumValue(double? currentValue, double valueToCheck)
        {
            if (currentValue == null || currentValue < valueToCheck)
                return valueToCheck;
            return currentValue;
        }

        private int? getWorstWeatherConditionLevel(int? currentRiskValue, int idValueToCheck)
        {
            int toCheckRiskLevel = -1;

            foreach (var list in listOfWeatherConditionsRisk)
            {
                if (list.Contains(idValueToCheck))
                {
                    if (RadioButtonWalking.IsChecked == true)
                        toCheckRiskLevel = list[1];
                    else
                        toCheckRiskLevel = list[2];
                }
            }

            if (currentRiskValue == null)
                return toCheckRiskLevel;

            else if (currentRiskValue < toCheckRiskLevel)
                return toCheckRiskLevel;

            return currentRiskValue;
        }


        private int getTemperatureRiskLevelForWalking(double? minTemperature, double? maxTemperature)
        {
            int riskLevel = -1;

            if ((minTemperature >= -5 && minTemperature <= 29) || (maxTemperature >= -5 && maxTemperature <= 29))
            {
                if (riskLevel < 0)
                    riskLevel = 0;
            }
            if (((minTemperature >= -15 && minTemperature < -5) || (minTemperature >= 30 && minTemperature < 34)) ||
                ((maxTemperature >= -15 && maxTemperature < -5) || (maxTemperature >= 30 && maxTemperature < 34)))
            {
                if (riskLevel < 1)
                    riskLevel = 1;
            }
            if ((minTemperature >= 34) || (maxTemperature >= 34))
            {
                riskLevel = 2;
            }

            return riskLevel;
        }

        private int getTemperatureRiskLevelForCycling(double? minTemperature, double? maxTemperature)
        {
            int riskLevel = -1;

            if ((minTemperature >= 0 && minTemperature <= 29) || (maxTemperature >= 0 && maxTemperature <= 29))
            {
                if (riskLevel < 0)
                    riskLevel = 0;
            }
            if (((minTemperature >= 30 && minTemperature <= 34) || (maxTemperature >= 30 && maxTemperature <= 34)))
            {
                if (riskLevel < 1)
                    riskLevel = 1;
            }
            if (((minTemperature <= 0) || (minTemperature > 34)) || ((maxTemperature <= 0) || (maxTemperature > 34)))
            {
                riskLevel = 2;
            }

            return riskLevel;
        }

        private int getCloudinessRiskLevelForWalkingAndCycling(double? maxCloudiness)
        {
            if (maxCloudiness != null)
            {
                if (maxCloudiness >= 84)
                    return 2;

                else if (maxCloudiness >= 60 && maxCloudiness <= 83)
                    return 1;

                else if (maxCloudiness <= 59)
                    return 0;
            }
            return -1;
        }

        private int getWindSpeedRiskLevelForWalking(double? maxWindSpeed)
        {
            double maxWindSpeedInKilometers = maxWindSpeed.Value * 3.6;

            if (maxWindSpeedInKilometers <= 58)
                return 0;

            else if (maxWindSpeedInKilometers >= 59 && maxWindSpeedInKilometers <= 69)
                return 1;

            else if (maxWindSpeedInKilometers >= 70)
                return 2;

            return -1;
        }

        private int getWindSpeedRiskLevelForCycling(double? maxWindSpeed)
        {
            double maxWindSpeedInKilometers = maxWindSpeed.Value * 3.6;

            if (maxWindSpeedInKilometers <= 48)
                return 0;

            else if (maxWindSpeedInKilometers >= 49 && maxWindSpeedInKilometers <= 59)
                return 1;

            else if (maxWindSpeedInKilometers >= 60)
                return 2;

            return -1;
        }

        private int getVisibilityRiskLevelForWalking(double? maxVisibility)
        {
            if (maxVisibility != null)
            {
                if (maxVisibility < 50)
                    return 2;

                else if (maxVisibility >= 50 && maxVisibility <= 200)
                    return 1;

                else if (maxVisibility > 200)
                    return 0;
            }
            return -1;
        }

        private int getVisibilityRiskLevelForCycling(double? maxVisibility)
        {
            if (maxVisibility != null)
            {
                if (maxVisibility < 50)
                    return 2;

                else if (maxVisibility >= 50 && maxVisibility <= 200)
                    return 1;

                else if (maxVisibility > 200)
                    return 0;
            }
            return -1;
        }
        private float? decimalToDM(double coord)
        {
            String output, degrees, minutes;

            double mod = coord % 1;
            int intPart = (int)coord;
            degrees = intPart.ToString();
            coord = mod * 60;
            mod = coord % 1;
            intPart = (int)coord;
            if (intPart < 0)
            {
                intPart *= -1;
            }
            minutes = intPart.ToString();
            output = degrees + "." + minutes;

            return float.Parse(output, CultureInfo.InvariantCulture.NumberFormat);
        }

        private void checkWarnings()
        {
            StringBuilder warningText = new StringBuilder();

            if (frostWarningLevel == 0 || heatWarningLevel == 0 ||
                    windWarningLevel == 0 || rainWarningLevel == 0 ||
                    stormWarningLevel == 0 || tornadoWarningLevel == 0)
            {
                ImageWarningRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_not.png", UriKind.Relative));
                warningText.Append(Properties.Resources.PlanTheTrip_Risk_None_Warnings);
            }

            else
            {
                /*if (!_riskPointGranted)
                {
                    riskPoints.setVariable(riskPoints.getValue() + 15);
                    _riskPointGranted = true;
                } */

                
                ImageWarningRisk.Source = new BitmapImage(new Uri(@"/Images/Risk_icons/if_risk_high.png", UriKind.Relative));
                RiskLevelAssessment += 15;
                switch (frostWarningLevel)
                {
                    case 1:
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Frost_Degree1);
                        break;
                    case 2:
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Frost_Degree2);
                        break;
                    case 3:
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Frost_Degree3);
                        break;
                }

                switch (heatWarningLevel)
                {
                    case 1:
                        warningText.Append(Environment.NewLine);
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Heat_Degree1);
                        break;
                    case 2:
                        warningText.Append(Environment.NewLine);
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Heat_Degree2);
                        break;
                    case 3:
                        warningText.Append(Environment.NewLine);
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Heat_Degree3);
                        break;
                }

                switch (windWarningLevel)
                {
                    case 1:
                        warningText.Append(Environment.NewLine);
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Wind_Degree1);
                        break;
                    case 2:
                        warningText.Append(Environment.NewLine);
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Wind_Degree2);
                        break;
                    case 3:
                        warningText.Append(Environment.NewLine);
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Wind_Degree3);
                        break;
                }

                switch (rainWarningLevel)
                {
                    case 1:
                        warningText.Append(Environment.NewLine);
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Rain_Degree1);
                        break;
                    case 2:
                        warningText.Append(Environment.NewLine);
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Rain_Degree2);
                        break;
                    case 3:
                        warningText.Append(Environment.NewLine);
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Rain_Degree3);
                        break;
                }

                switch (stormWarningLevel)
                {
                    case 1:
                        warningText.Append(Environment.NewLine);
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Storms_Degree1);
                        break;
                    case 2:
                        warningText.Append(Environment.NewLine);
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Storms_Degree2);
                        break;
                    case 3:
                        warningText.Append(Environment.NewLine);
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Storms_Degree3);
                        break;
                }

                switch (tornadoWarningLevel)
                {
                    case 1:
                        warningText.Append(Environment.NewLine);
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Tornado_Degree1);
                        break;
                    case 2:
                        warningText.Append(Environment.NewLine);
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Tornado_Degree2);
                        break;
                    case 3:
                        warningText.Append(Environment.NewLine);
                        warningText.Append(Properties.Resources.PlanTheTrip_Risk_Tornado_Degree3);
                        break;
                }
            }

            TextWarningRisk.Text = warningText.ToString();
        }

        private void updateRiskAssessment()
        {
            if (RiskLevelAssessment <= 4)
                RiskAssessment.Text = "Brak ryzyka (" + RiskLevelAssessment + "pkt)";

            else if (RiskLevelAssessment >= 15)
                RiskAssessment.Text = "Duże ryzyko (" + RiskLevelAssessment + "pkt)";
            else
                RiskAssessment.Text = "Umiarkowane ryzyko (" + RiskLevelAssessment + "pkt)";
        }
        private void initializeListOfWeatherConditionsRisk()
        {
            //first column - Condition ID; second column - walking risk level; third column - cycling risk level
            // 0 - none risk level; 1 - moderate risk level; 2 - high risk level

            ///Group 2xx: Thunderstorm
            listOfWeatherConditionsRisk = new List<List<int>>();

            listOfWeatherConditionsRisk.Add(new List<int> {200, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 201, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 202, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 210, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 211, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 212, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 221, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 230, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 231, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 232, 2, 2});

            ///Group 3xx: Drizzle
            listOfWeatherConditionsRisk.Add(new List<int> { 300, 1, 1});
            listOfWeatherConditionsRisk.Add(new List<int> { 301, 1, 1});
            listOfWeatherConditionsRisk.Add(new List<int> { 302, 1, 1});
            listOfWeatherConditionsRisk.Add(new List<int> { 310, 1, 1});
            listOfWeatherConditionsRisk.Add(new List<int> { 311, 1, 1});
            listOfWeatherConditionsRisk.Add(new List<int> { 312, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 313, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 314, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 321, 2, 2});

            ///Group 5xx: Rain
            listOfWeatherConditionsRisk.Add(new List<int> { 500, 0, 1});
            listOfWeatherConditionsRisk.Add(new List<int> { 501, 1, 2});
            listOfWeatherConditionsRisk.Add(new List<int> {502, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 503, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 504, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 511, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 520, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 521, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 522, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 531, 2, 2});
        
            ///Group 6xx: Snow
            listOfWeatherConditionsRisk.Add(new List<int> { 600, 1, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 601, 1, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 602, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 611, 1, 1});
            listOfWeatherConditionsRisk.Add(new List<int> { 612, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 615, 1, 1});
            listOfWeatherConditionsRisk.Add(new List<int> { 616, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 620, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 621, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 622, 2, 2});
        
            ///Group 7xx: Atmosphere
            listOfWeatherConditionsRisk.Add(new List<int> { 701, 0, 1});
            listOfWeatherConditionsRisk.Add(new List<int> { 711, 0, 0});
            listOfWeatherConditionsRisk.Add(new List<int> { 721, 1, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 731, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 741, 1, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 751, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 761, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 762, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 771, 2, 2});
            listOfWeatherConditionsRisk.Add(new List<int> { 781, 2, 2});
        
            ///Group 800: Clear
            listOfWeatherConditionsRisk.Add(new List<int> { 800, 0, 0});

            ///Group 80x: Clouds
            listOfWeatherConditionsRisk.Add(new List<int> { 801, 0, 0});
            listOfWeatherConditionsRisk.Add(new List<int> { 802, 0, 0});
            listOfWeatherConditionsRisk.Add(new List<int> { 803, 1, 1});
            listOfWeatherConditionsRisk.Add(new List<int> { 804, 2, 2});
        }

        private void browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            browser.ExecuteScriptAsync("initializeMap();");
        }
    }
}
