namespace PolygonGeneralization.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Maps",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Polygons",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        MapId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Maps", t => t.MapId, cascadeDelete: true)
                .Index(t => t.MapId);
            
            CreateTable(
                "dbo.Paths",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PolygonId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Polygons", t => t.PolygonId, cascadeDelete: true)
                .Index(t => t.PolygonId);
            
            CreateTable(
                "dbo.Points",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PathId = c.Guid(nullable: false),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Paths", t => t.PathId, cascadeDelete: true)
                .Index(t => t.PathId)
                .Index(t => t.X, name: "IX_Points_X")
                .Index(t => t.Y, name: "IX_Points_Y");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Polygons", "MapId", "dbo.Maps");
            DropForeignKey("dbo.Paths", "PolygonId", "dbo.Polygons");
            DropForeignKey("dbo.Points", "PathId", "dbo.Paths");
            DropIndex("dbo.Points", "IX_Points_Y");
            DropIndex("dbo.Points", "IX_Points_X");
            DropIndex("dbo.Points", new[] { "PathId" });
            DropIndex("dbo.Paths", new[] { "PolygonId" });
            DropIndex("dbo.Polygons", new[] { "MapId" });
            DropTable("dbo.Points");
            DropTable("dbo.Paths");
            DropTable("dbo.Polygons");
            DropTable("dbo.Maps");
        }
    }
}
