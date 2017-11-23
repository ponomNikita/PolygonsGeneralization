using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;
using Polygon = PolygonGeneralization.Domain.Models.Polygon;

namespace PolygonGeneralization.Infrastructure.Services
{
    public class GeoJson
    {
        public Feature[] Features { get; set; }
    }

    public class Feature
    {
        [JsonConverter(typeof(GeometryJsonConverter))]
        public Geometry Geometry { get; set; }
    }

    public class Geometry
    {
        public double[][][] Coordinates { get; set; }
    }

    public class GeometryJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

            if ((string)obj["type"] == "Polygon")
            {
                var geometry = obj.ToObject<Geometry>();

                return geometry;
            }
            
            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Geometry);
        }
    }
    public class JsonGisDataReader : IGisDataReader
    {
        public Map ReadFromFile(string filename)
        {
            var result = new Map(filename);

            var serializer = new JsonSerializer();

            using (var file = File.OpenText(filename))
            using(var reader = new JsonTextReader(file))
            {
                var geoJson = serializer.Deserialize<GeoJson>(reader);

                var polygons = geoJson.Features
                    .Select(f => f.Geometry)
                    .Select(g => g != null ? new Polygon(g.Coordinates) : null) 
                    // TODO убрать последнюю точку в контуре (дублирует начальную)
                    .Where(p => p != null)
                    .ToArray();

                foreach (var polygon in polygons)
                {
                    polygon.TransformToR3();
                    polygon.MapId = result.Id;
                    result.Polygons.Add(polygon);
                }

                return result;
            }
        }
    }
}
