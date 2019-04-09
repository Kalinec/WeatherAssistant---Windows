using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Navigation;

namespace weatherAssistant.Helpers.Converters
{
    /// <summary>
    /// OpenWeatherMap API returns weather condition always in English, so I have to translate it manually... :|
    /// </summary>
    public class EnglishToPolishConditionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string notTranslatedConditionWeather = value.ToString();

                if (Thread.CurrentThread.CurrentCulture.Name == "pl-PL")
                {
                    switch (notTranslatedConditionWeather)
                    {
                        case "Thunderstorm":
                            return "Burza";

                        case "Drizzle":
                            return "Mżawka";

                        case "Rain":
                            return "Deszcz";

                        case "Snow":
                            return "Śnieg";

                        case "Atmosphere":
                            return "Aura";

                        case "Clear":
                            return "Czyste niebo";

                        case "Clouds":
                            return "Chmury";

                        case "Mist":
                            return "Mglisto";

                        case "Fog":
                            return "Mglisto";

                        default:
                            return "zmienili Ci API KAROL";
                    }
                }

                else
                    return notTranslatedConditionWeather;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
