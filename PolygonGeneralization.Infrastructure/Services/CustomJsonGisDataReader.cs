using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;
using Path = PolygonGeneralization.Domain.Models.Path;

namespace PolygonGeneralization.Infrastructure.Services
{
    public class CustomJsonGisDataReader : IGisDataReader
    {
        public Map ReadFromFile(string filename)
        {
            var json = File.ReadAllText(filename);

            var polygonDtos = JsonConvert.DeserializeObject<List<PolygonDto>>(json);

            var map = new Map(filename);
            map.Polygons = polygonDtos.Select(it => new Polygon(
                new Path(it.Points.Select(p => new Point(p.X, p.Y)).ToArray()))).ToList();

            return map;
        }
        private class PolygonDto
        {
            public System.Drawing.Point[] Points { get; set; }
        }
    }
}