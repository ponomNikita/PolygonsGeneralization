using PolygonGeneralization.Domain;
using PolygonGeneralization.Domain.Interfaces;
using SharpMap.Data;
using SharpMap.Data.Providers;
using Polygon = PolygonGeneralization.Domain.Models.Polygon;

namespace PolygonGeneralization.Infrastructure.Services
{
    public class SharpMapGisDataReader : IGisDataReader
    {
        public Polygon[] ReadFromFile(string filename)
        {
            //var file = new ShapeFile(filename);

            //var bbox = file.GetExtents();

            //FeatureDataSet dataSet = new FeatureDataSet();

            //file.ExecuteIntersectionQuery(bbox, dataSet);

            //var table = dataSet.Tables[0];

            //foreach (FeatureDataRow row in table.Rows)
            //{
            //    var poly = row.Geometry;
            //}
            throw new System.NotImplementedException();
        }
    }
}
