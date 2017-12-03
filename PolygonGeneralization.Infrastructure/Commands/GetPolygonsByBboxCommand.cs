using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Infrastructure.Commands
{
    public class GetPolygonsByBboxCommand : BaseCommand
    {
        private BaseSelectCommand _selectCommand;

        public GetPolygonsByBboxCommand(Guid mapId, Point leftDown, Point rightTop)
        {
            var commandText = $@"
                SELECT points.X, points.Y, points.OrderNumber,
		        paths.Id as PathId,
		        paths.PolygonId as PolygonId 
		        FROM [dbo].[Points] points
		        JOIN [dbo].[Paths] paths on points.PathId = paths.Id 
		        JOIN [dbo].[Polygons] polygons on polygons.Id = paths.PolygonId
		        JOIN [dbo].[Maps] m on m.Id = polygons.MapId
		        WHERE 
                m.Id = '{mapId}' AND
		        points.X >= {leftDown.X.ToString(CultureInfo.InvariantCulture)} AND
		        points.X <= {rightTop.X.ToString(CultureInfo.InvariantCulture)} AND
		        points.Y >= {leftDown.Y.ToString(CultureInfo.InvariantCulture)} AND
		        points.Y <= {rightTop.Y.ToString(CultureInfo.InvariantCulture)}";

            _selectCommand = new BaseSelectCommand(commandText, "Selecting query") {LoggingOn = false};

            Polygons = new Collection<Polygon>();
        }

        public override string CommandName => "Building polygons by bbox";

        public ICollection<Polygon> Polygons { get; private set; }

        protected override void HandleImpl()
        {
            _selectCommand.Execute();

            var geometries = _selectCommand.Result.AsEnumerable().Select(r => new Geometry()
            {
                X = (double)r["X"],
                Y = (double)r["Y"],
                OrderNumber = (int)r["OrderNumber"],
                PathId = Guid.Parse(r["PathId"].ToString()),
                PolygonId = Guid.Parse(r["PolygonId"].ToString())
            });

            Polygons = new List<Polygon>();

            foreach (var pol in geometries.GroupBy(g => g.PolygonId).ToList())
            {
                var polygon = new Polygon();
                
                foreach (var pth in pol.GroupBy(g => g.PathId).ToList())
                {
                    var path = new Path(pth.OrderBy(p => p.OrderNumber).Select(p => new Point(p.X, p.Y) {OrderNumber = p.OrderNumber})
                        .ToArray());

                    polygon.AddPath(path);
                }
                Polygons.Add(polygon);
            }
        }

        private class Geometry
        {
            public double X { get; set; }
            public double Y { get; set; }
            public int OrderNumber { get; set; }
            public Guid PathId { get; set; }
            public Guid PolygonId { get; set; }
        }
    }
}