using System.Configuration;
using System.Data;
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

            var connectionString = ConfigurationManager.ConnectionStrings[_dbName].ConnectionString;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = CommandText;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}