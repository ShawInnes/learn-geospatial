using System.Collections.Generic;
using System.Linq;
using H3;
using NetTopologySuite.Geometries;

namespace api.Data
{
    public class ClusterItem
    {
        public string Index { get; set; }
        public Point Centroid { get; set; }
        public Geometry Boundary { get; set; }
        public int Count { get; set; }
    }
}
