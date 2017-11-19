using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using PolygonGeneralization.Domain.Interfaces;

namespace PolygonGeneralization.Infrastructure.Commands
{
    public abstract class BaseDbCommand : ICommand
    {
        private string _dbName;

        protected BaseDbCommand(string dbName)
        {
            _dbName = dbName;
        }

        protected abstract string CommandText { get; }

        public void Handle()
        {
            SQLiteConnection.CreateFile(_dbName);

            SQLiteFactory factory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");

            using (var connection = (SQLiteConnection)factory.CreateConnection())
            {
                if (connection == null)
                {
                    throw new Exception("Could not create connetion");
                }

                connection.ConnectionString = $"Data Source = {_dbName}";
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = CommandText;

                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}