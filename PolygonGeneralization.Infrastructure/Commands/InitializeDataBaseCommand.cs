using PolygonGeneralization.Domain.Interfaces;

namespace PolygonGeneralization.Infrastructure.Commands
{
    public class InitializeDataBaseCommand : BaseDbCommand
    {
        public InitializeDataBaseCommand(string dbName) : base(dbName)
        {
        }

        protected override string CommandText => @"CREATE TABLE IF NOT EXISTS [Coordinates](
                                            [X] REAL NOT NULL,
                                            [Y] REAL NOT NULL,
                                            PRIMARY KEY([X], [Y])
                                            );";
    }
}