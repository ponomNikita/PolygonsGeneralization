namespace PolygonGeneralization.Infrastructure.Commands
{
    public class InitializeDataBaseCommand : BaseDbCommand
    {
        public InitializeDataBaseCommand(string dbName) : base(dbName)
        {
        }

        protected override string CommandText =>
            @"                                          
            CREATE TABLE IF NOT EXISTS [Coordinates](
            [Id] INTEGER PRIMARY KEY AUTOINCREMENT,
            [X] REAL NOT NULL,
            [Y] REAL NOT NULL
            );

            CREATE INDEX IF NOT EXISTS IX_Coordinates_Id ON [Coordinates] ([Id]);
            CREATE INDEX IF NOT EXISTS IX_Coordinates_Point ON [Coordinates] (X, Y);

            CREATE TABLE IF NOT EXISTS [Map](
            [Id]  INTEGER PRIMARY KEY AUTOINCREMENT,
            [Name] TEXT NOT NULL,
            [LeftDownPoint] INTEGER NOT NULL,            
            [RightTopPoint] INTEGER NOT NULL,
            [ZeroPoint] INTEGER NOT NULL,

            FOREIGN KEY(LeftDownPoint) REFERENCES Coordinates(Id),
            FOREIGN KEY(RightTopPoint) REFERENCES Coordinates(Id),
            FOREIGN KEY(ZeroPoint) REFERENCES Coordinates(Id)
            );

            CREATE INDEX IF NOT EXISTS IX_Map_Id ON [Map] (Id);
            CREATE INDEX IF NOT EXISTS IX_Map_LeftDownPoint ON [Map] (LeftDownPoint);
            CREATE INDEX IF NOT EXISTS IX_Map_RightTopPoint ON [Map] (RightTopPoint);
            CREATE INDEX IF NOT EXISTS IX_Map_ZeroPoint ON [Map] (ZeroPoint);";
    }
}