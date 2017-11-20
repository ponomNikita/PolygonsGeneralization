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
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Polygons",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Map_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Maps", t => t.Map_Id, cascadeDelete: true)
                .Index(t => t.Map_Id);
            
            CreateTable(
                "dbo.Paths",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Polygon_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Polygons", t => t.Polygon_Id)
                .Index(t => t.Polygon_Id);
            
            CreateTable(
                "dbo.Points",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Path_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Paths", t => t.Path_Id)
                .Index(t => t.X, name: "IX_Points_X")
                .Index(t => t.Y, name: "IX_Points_Y")
                .Index(t => t.Path_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Polygons", "Map_Id", "dbo.Maps");
            DropForeignKey("dbo.Paths", "Polygon_Id", "dbo.Polygons");
            DropForeignKey("dbo.Points", "Path_Id", "dbo.Paths");
            DropIndex("dbo.Points", new[] { "Path_Id" });
            DropIndex("dbo.Points", "IX_Points_Y");
            DropIndex("dbo.Points", "IX_Points_X");
            DropIndex("dbo.Paths", new[] { "Polygon_Id" });
            DropIndex("dbo.Polygons", new[] { "Map_Id" });
            DropTable("dbo.Points");
            DropTable("dbo.Paths");
            DropTable("dbo.Polygons");
            DropTable("dbo.Maps");
        }
    }
}
