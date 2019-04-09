using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using weatherAssistant.Helpers.Interfaces;

namespace weatherAssistant.Helpers
{
    public class MapBoxQueryBuilder : IMapBoxQueryBuilder
    {
        private NumberFormatInfo separator;

        public MapBoxQueryBuilder()
        {
            separator = new NumberFormatInfo();
            separator.NumberDecimalSeparator = ".";
        }

        public string Url { get; private set; }

        public string ForwardGeocoder([NotNull] string place)
        {
            Url = string.Format("https://api.mapbox.com/geocoding/v5/mapbox.places/{0}.json?access_token={1}",
                place,
                Properties.Settings.Default.MapBox_APPID);

            return Url;
        }

        public string Direction([NotNull] string type, [NotNull] double originLatitude,
            [NotNull] double originLongitude, [NotNull] double destinationLatitude,
            [NotNull] double destinationLongitude)
        {
            Url = string.Format("https://api.mapbox.com/directions/v5/mapbox/{0}/{1},{2};{3},{4}?overview=full&annotations=distance&geometries=geojson&access_token={5}",
                type,
                originLatitude.ToString(separator),
                originLongitude.ToString(separator),
                destinationLatitude.ToString(separator),
                destinationLongitude.ToString(separator),
                Properties.Settings.Default.MapBox_APPID);

            return Url;
        }
    }
}
