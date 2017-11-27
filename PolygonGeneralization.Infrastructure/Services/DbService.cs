using System;
using System.Data.Entity;
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
    }
}