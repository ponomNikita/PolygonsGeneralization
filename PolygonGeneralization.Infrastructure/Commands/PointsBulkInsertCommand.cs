using System;
using System.Data;
using System.Linq;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Infrastructure.Commands
{
    public class PointsBulkInsertCommand : BulkInsertCommand
    {
        private readonly Map _map;

        public PointsBulkInsertCommand(Map map)
        {
            _map = map;
        }

        public override string TableName => "dbo.Points";
        public override string EntityName => nameof(Point);
        protected override DataTable GetData()
        {
            var result = new DataTable("Points");

            result.Columns.Add(new DataColumn("Id", typeof(Guid)));
            result.Columns.Add(new DataColumn("PathId", typeof(Guid)));
            result.Columns.Add(new DataColumn("X", typeof(double)));
            result.Columns.Add(new DataColumn("Y", typeof(double)));
            result.Columns.Add(new DataColumn("OrderNumber", typeof(int)));

            var points = _map.Polygons.SelectMany(p => p.Paths).SelectMany(p => p.Points).ToList();
            foreach (var point in points)
            {
                var row = result.NewRow();
                row["Id"] = point.Id;
                row["PathId"] = point.PathId;
                row["X"] = point.X;
                row["Y"] = point.Y;
                row["OrderNumber"] = point.OrderNumber;

                result.Rows.Add(row);
            }
            result.AcceptChanges();

            return result;
        }
    }
}