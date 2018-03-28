using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PolygonGeneralization.Domain.Exceptions;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.Infrastructure.Commands;

namespace PolygonGeneralization.Infrastructure.Services
{
    public class DbService : IDbService, IDisposable
    {
        private readonly DbContext _context;

        public DbService(DbContext context)
        {
            _context = context;
            _context.Database.Log = Console.WriteLine;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveMap(Map map)
        {
            var saveMetaCommand = new SaveMapMetaCommand(map);
            var polygonsInsertCommand = new PolygonsBulkInsertCommand(map);
            var pathsInsertCommand = new PathsBulkInsertCommand(map);
            var pointsInsertCommand = new PointsBulkInsertCommand(map);

            saveMetaCommand.Execute();
            polygonsInsertCommand.Execute();
            pathsInsertCommand.Execute();
            pointsInsertCommand.Execute();
        }

        public IEnumerable<Map> GetMaps()
        {
            return _context.Set<Map>().AsEnumerable();
        }

        public double[] GetExtrimalPoints(Guid mapId)
        {
            var commandText = $@"SELECT 
	                            MIN(X) as minX, 
	                            MIN(Y) as minY,
	                            MAX(X) as maxX,
	                            MAX(Y) as maxY
	                            FROM
	                            (	
		                            SELECT points.X, points.Y
		                            FROM [dbo].[Points] points
		                            JOIN [dbo].[Paths] paths on points.PathId = paths.Id 
		                            JOIN [dbo].[Polygons] polygons on polygons.Id = paths.PolygonId
		                            JOIN [dbo].[Maps] m on m.Id = polygons.MapId
		                            WHERE m.Id = '{mapId}'
	                            ) as t";

            var command = new BaseSelectCommand(commandText, "Selecting extrimal points");
            command.Execute();

            var data = command.Result;

            if (data.Rows.Count != 1)
            {
                throw new PolygonGeneralizationException("Ожидалась одна строка данных");
            }

            var result = data.Rows[0].ItemArray.Cast<double>().ToArray();
            
            return result;
        }

        public Polygon[] GetPolygons(Guid mapId, Point leftDown, Point rightTop)
        {
            var command = new GetPolygonsByBboxCommand(mapId, leftDown, rightTop);

            command.Execute();

            return command.Polygons.ToArray();
        }
    }
}