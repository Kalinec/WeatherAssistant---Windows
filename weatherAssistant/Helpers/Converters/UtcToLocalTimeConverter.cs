using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace weatherAssistant.Helpers.Converters
{
    class UtcToLocalTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string date = value.ToString();

            if (date.Equals("0"))
                return null;

            DateTime utc = DateTime.Parse(value.ToString());
            DateTime local = utc.ToLocalTime();
            return local.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
