using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using api.Data;
using H3;
using H3.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetTopologySuite;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.Shape;
using NetTopologySuite.Utilities;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> _logger;
        private readonly PostGisDbContext _dbContext;

        public DataController(ILogger<DataController> logger, PostGisDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<PublicArt> Get()
        {
            return _dbContext.PublicArts.ToList();
        }

        [HttpGet("cluster")]
        public FeatureCollection Clustered([FromQuery] int zoom = 13, [FromQuery] int resolution = 5, [FromQuery] string bbox = null)
        {
            var featureCollection = new FeatureCollection();
            var bboxList = JsonSerializer.Deserialize<List<double>>($"[{bbox}]");

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var boundingPoly = geometryFactory.CreatePolygon(new[]
            {
                new Coordinate(bboxList[0], bboxList[1]),
                new Coordinate(bboxList[2], bboxList[1]),
                new Coordinate(bboxList[2], bboxList[3]),
                new Coordinate(bboxList[0], bboxList[3]),
                new Coordinate(bboxList[0], bboxList[1])
            });

            var itemQuery = _dbContext.PublicArts
                .Where(p => p.TheGeom.Within(boundingPoly))
                .AsEnumerable();

            if (zoom < 17)
            {
                _logger.LogInformation("Returning Clustered Data");
                var cluster = itemQuery
                    .GroupBy(p => H3Index.FromPoint((Point)p.TheGeom, resolution))
                    .Select(p => new ClusterItem
                    {
                        Index = p.Key.ToString(),
                        Centroid = p.Key.ToPoint(),
                        Boundary = p.Key.GetCellBoundary(),
                        Count = p.Count(),
                    }).ToList();

                foreach (var item in cluster)
                {
                    featureCollection.Add(new Feature(item.Boundary, new AttributesTable(new Dictionary<string, object>()
                    {
                        { "Index", item.Index },
                        { "Count", item.Count },
                    })));
                }
            }
            else
            {
                _logger.LogInformation("Returning Full Data Set");
                foreach (var item in itemQuery)
                {
                    featureCollection.Add(new Feature(item.TheGeom, new AttributesTable(new Dictionary<string, object>
                    {
                        { "Title", item.Title },
                        { "Artist", item.Artist },
                        { "Description", item.Artist },
                        { "Year", item.Year },
                        { "Index", H3Index.FromPoint((Point)item.TheGeom, resolution) },
                        { "Count", 1 },
                    })));
                }
            }


            return featureCollection;
        }
    }
}
