using System;
using System.Configuration;
using System.Data;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Infrastructure.Logger;

namespace PolygonGeneralization.Infrastructure.Commands
{
    public abstract class BulkInsertCommand : BaseCommand
    {
        private const int TIMEOUT = 240;

        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Polygons"].ConnectionString;
        private readonly ILogger _logger = LoggerFactory.Create();

        public override string CommandName => $"Inserting {EntityName}s to database";
        protected override void HandleImpl()
        {
            var data = GetData();
            using (var bulkCopy = new SqlBulkCopy(_connectionString))
            {
                bulkCopy.DestinationTableName = TableName;
                bulkCopy.BulkCopyTimeout = TIMEOUT;

                try
                {
                    bulkCopy.WriteToServer(data);
                }
                catch (Exception ex)
                {
                    _logger.Log($"Error while saving data: {ex}");
                }
            }
        }

        public abstract string TableName { get;}
        public abstract string EntityName { get; }
        protected abstract DataTable GetData();
    }
}