using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Infrastructure.Commands;

namespace PolygonGeneralization.Infrastructure.Services
{
    public class DbService : IDbService
    {
        public void InitializeDataBase(string dbName)
        {
            var initializeDataBaseCommand = new InitializeDataBaseCommand(dbName);

            initializeDataBaseCommand.Handle();
        }
    }
}