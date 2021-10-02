using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using H3;
using NetTopologySuite.Geometries;

#nullable disable

namespace api.Data
{
    public partial class PublicArt
    {
        public int Gid { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Location { get; set; }
        public string Material { get; set; }
        public string Description { get; set; }
        public int? Year { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public Geometry TheGeom { get; set; }
    }
}
