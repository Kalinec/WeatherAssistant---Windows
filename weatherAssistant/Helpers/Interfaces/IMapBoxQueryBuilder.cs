using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weatherAssistant.Helpers.Interfaces
{
    public interface IMapBoxQueryBuilder
    {
        string Url { get; }

        string ForwardGeocoder(string place);

        string Direction(string type, double originLatitude, double originLongitude, double destinationLatitude,
            double destinationLongitude);
    }
}
