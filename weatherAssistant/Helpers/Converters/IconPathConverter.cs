using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace weatherAssistant.Helpers.Converters
{
    class IconPathConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch (value.ToString())
                {
                    case "01d":
                        return "../Images/Weather_Icons/if_sunny.png";

                    case "01n":
                        return "../Images/Weather_Icons/if_night.png";

                    case "02d":
                        return "../Images/Weather_Icons/if_cloudy_day.png";

                    case "02n":
                        return "../Images/Weather_Icons/if_cloudy_night.png";

                    case "03d":
                        return "../Images/Weather_Icons/if_cloudy.png";

                    case "03n":
                        return "../Images/Weather_Icons/if_cloudy.png";

                    case "04d":
                        return "../Images/Weather_Icons/if_cloudy.png";

                    case "04n":
                        return "../Images/Weather_Icons/if_cloudy.png";

                    case "09d":
                        return "../Images/Weather_Icons/if_cloudy_rainy.png";

                    case "09n":
                        return "../Images/Weather_Icons/if_cloudy.png";

                    case "10d":
                        return "../Images/Weather_Icons/if_cloudy_rainy_day.png";

                    case "10n":
                        return "../Images/Weather_Icons/if_cloudy_rainy_night.png";

                    case "11d":
                        return "../Images/Weather_Icons/if_cloudy_storm.png";

                    case "11n":
                        return "../Images/Weather_Icons/if_cloudy_storm.png";

                    case "13d":
                        return "../Images/Weather_Icons/if_cloudy_snowy_day.png";

                    case "13n":
                        return "../Images/Weather_Icons/if_cloudy_snowy.png";

                    case "50d":
                        return "../Images/Weather_Icons/if_windyOrMist.png";

                    case "50n":
                        return "../Images/Weather_Icons/if_windyOrMist.png";
                    default:
                        return null;

                }
            }

            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
