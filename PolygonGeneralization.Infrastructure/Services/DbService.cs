using System;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.Infrastructure.Commands;
using PolygonGeneralization.Infrastructure.Logger;

namespace PolygonGeneralization.Infrastructure.Services
{
    public class DbService : IDbService, IDisposable
    {
        private readonly DbContext _context;

        public DbService(DbContext context)
        {
            _context = context;
        }

        //public void SaveMap(Map map)
        //{
        //    _logger.Log("Creating transaction");
        //    var transaction = _context.Database.BeginTransaction();
        //    _logger.Log("Done");

        //    _context.Configuration.AutoDetectChangesEnabled = false;
        //    _context.Configuration.ValidateOnSaveEnabled = false;

        //    try
        //    {
        //        var isertedMap = _context.Set<Map>().Add(new Map(map.Name));
        //        _context.SaveChanges();

        //        _logger.Log("Processing polygons");
        //        int counter = 1;
        //        foreach (var polygon in map.Polygons)
        //        {
        //            polygon.Map = isertedMap;
        //            _context.Set<Polygon>().Add(polygon);

        //            if (counter%500 == 0)
        //            {
        //                _logger.Log("Saving changes");
        //                _context.SaveChanges();
        //            }
        //            _logger.Log($"Processed polygons count {counter}");
        //            counter++;
        //        }

        //        _logger.Log("Saving changes");
        //        _context.SaveChanges();
        //        _logger.Log("Done");

        //        _logger.Log("Commiting transaction");
        //        transaction.Commit();
        //        _logger.Log("Done");

        //    }
        //    finally
        //    {
        //        _context.Configuration.AutoDetectChangesEnabled = true;
        //        _context.Configuration.ValidateOnSaveEnabled = true;
        //        transaction.Dispose();
        //    }


        //}

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