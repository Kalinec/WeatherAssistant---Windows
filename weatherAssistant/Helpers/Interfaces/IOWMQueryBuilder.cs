using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace weatherAssistant.Helpers.Interfaces
{
    public interface IOWMQueryBuilder
    {
        string CurrentWeatherByCityName([NotNull] string city);
        string ForecastWeatherByCityName([NotNull] string city);
        string CurrentWeatherByCoordinate([NotNull] double longitude, [NotNull] double latitude);

        string CitiesInRectangleZone([NotNull] string longitudeLeft, [NotNull] string latitudeBottom,
            [NotNull] string longitudeRight, [NotNull] string latitudeTop, [NotNull] string zoom);

    }
}
