using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weatherAssistant.Models
{
    public sealed class MapBoxAPI_Directions
    {
        public class Geometry
        {
            public List<List<double>> coordinates { get; set; }
            public string type { get; set; }
        }

        public class Annotation
        {
            public List<double> distance { get; set; }
        }

        public class Leg
        {
            public Annotation annotation { get; set; }
            public string summary { get; set; }
            public double weight { get; set; }
            public double duration { get; set; }
            public List<object> steps { get; set; }
            public double distance { get; set; }
        }

        public class Route
        {
            public Geometry geometry { get; set; }
            public List<Leg> legs { get; set; }
            public string weight_name { get; set; }
            public double weight { get; set; }
            public double duration { get; set; }
            public double distance { get; set; }
        }

        public class Waypoint
        {
            public string name { get; set; }
            public List<double> location { get; set; }
        }

        public class Root
        {
            public List<Route> routes { get; set; }
            public List<Waypoint> waypoints { get; set; }
            public string code { get; set; }
            public string uuid { get; set; }
        }
    }
}
