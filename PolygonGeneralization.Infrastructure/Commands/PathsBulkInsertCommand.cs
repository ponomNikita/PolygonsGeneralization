using System;
using System.Data;
using System.Linq;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Infrastructure.Commands
{
    public class PathsBulkInsertCommand : BulkInsertCommand
    {
        private readonly Map _map;

        public PathsBulkInsertCommand(Map map)
        {
            _map = map;
        }

        public override string TableName => "dbo.Paths";
        public override string EntityName => nameof(Path);
        protected override DataTable GetData()
        {
            var result = new DataTable("Paths");

            result.Columns.Add(new DataColumn("Id", typeof(Guid)));
            result.Columns.Add(new DataColumn("PolygonId", typeof(Guid)));

            var paths = _map.Polygons.SelectMany(p => p.Paths).ToList();

            foreach (var path in paths)
            {
                var row = result.NewRow();
                row["Id"] = path.Id;
                row["PolygonId"] = path.PolygonId;

                result.Rows.Add(row);
            }
            result.AcceptChanges();

            return result;
        }
    }
}