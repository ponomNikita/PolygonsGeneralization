using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

            saveMetaCommand.Handle();
            polygonsInsertCommand.Handle();
            pathsInsertCommand.Handle();
            pointsInsertCommand.Handle();
        }

        public IEnumerable<Map> GetMaps()
        {
            return _context.Set<Map>().AsEnumerable();
        }

        public Map GetMap(Guid mapId)
        {
            return _context.Set<Map>()
                .Include(m => m.Polygons)
                .Include(m => m.Polygons.Select(p => p.Paths))
                .Include(m => m.Polygons.Select(p => p.Paths.Select(path => path.Points)))
                .First(m => m.Id == mapId);
        }
    }
}