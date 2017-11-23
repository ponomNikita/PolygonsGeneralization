using System;
using System.Data;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Infrastructure.Commands
{
    public class PolygonsBulkInsertCommand : BulkInsertCommand
    {
        private readonly Map _map;

        public PolygonsBulkInsertCommand(Map map)
        {
            _map = map;
        }
        public override string TableName => "dbo.Polygons";
        public override string EntityName => nameof(Polygon);
        protected override DataTable GetData()
        {
            var result = new DataTable("Polygons");

            result.Columns.Add(new DataColumn("Id", typeof(Guid)));
            result.Columns.Add(new DataColumn("MapId", typeof(Guid)));

            foreach (var polygon in _map.Polygons)
            {
                var row = result.NewRow();
                row["Id"] = polygon.Id;
                row["MapId"] = polygon.MapId;

                result.Rows.Add(row);
            }
            result.AcceptChanges();

            return result;
        }
    }
}