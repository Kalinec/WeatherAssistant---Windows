using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace weatherAssistant.Helpers.Converters
{
    public class DegressToDirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] directions =
            {
                Properties.Resources.Direction_North,
                Properties.Resources.Direction_NorthEast,
                Properties.Resources.Direction_East,
                Properties.Resources.Direction_SouthEast,
                Properties.Resources.Direction_South,
                Properties.Resources.Direction_SouthWest,
                Properties.Resources.Direction_West,
                Properties.Resources.Direction_NorthWest,
                Properties.Resources.Direction_North
            };

            return directions[(int) Math.Round((((double)value % 360) / 45))];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
