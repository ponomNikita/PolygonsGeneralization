using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Infrastructure.Commands
{
    public class SaveMapMetaCommand : BaseCommand
    {
        private readonly string _connectionString;
        private Map _map;

        public SaveMapMetaCommand(Map map)
        {
            _map = map;
            _connectionString = ConfigurationManager.ConnectionStrings["Polygons"].ConnectionString;
        }
        public override string CommandName => "Saving meta information";
        protected override void HandleImpl()
        {
             var commandText = $"Insert into dbo.Maps Values('{_map.Id}', '{_map.Name}')";
            var connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }
    }
}