using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weatherAssistant.Helpers;
using weatherAssistant.Helpers.Interfaces;
using weatherAssistant.Models;

namespace weatherAssistant.Services
{
    public sealed class MapBoxService
    {
        private MapBoxQueryBuilder _queryBuilder;
        private IJsonDownloader _jsonDownloader;

        public MapBoxService(MapBoxQueryBuilder queryBuilder, IJsonDownloader jsonDownloader)
        {
            _queryBuilder = queryBuilder;
            _jsonDownloader = jsonDownloader;
        }

        public async Task<MapBoxAPI_Geocoder.Root> getForwardGeocoder(string place)
        {
            return await Task.Run(() =>
            {
                string url = _queryBuilder.ForwardGeocoder(place);

                var OutPut = _jsonDownloader.Download<MapBoxAPI_Geocoder.Root>(url);
                return OutPut;
            });
        }

        public async Task<MapBoxAPI_Directions.Root> getDirections(string type, double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude)
        {
            return await Task.Run(() =>
            {
                string url = _queryBuilder.Direction(type, originLatitude, originLongitude, destinationLatitude,
                    destinationLongitude);

                var OutPut = _jsonDownloader.Download<MapBoxAPI_Directions.Root>(url);
                return OutPut;
            });
        }
    }
}
