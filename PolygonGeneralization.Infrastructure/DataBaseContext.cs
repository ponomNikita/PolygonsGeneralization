using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Infrastructure
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext() : base("Polygons")
        { 
        }

        public DbSet<Map> Maps { get; set; }
        public DbSet<Polygon> Polygons { get; set; }
        public DbSet<Path> Paths { get; set; }
        public DbSet<Point> Points { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Map>().HasKey(e => e.Id);
            modelBuilder.Entity<Map>().Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); 
            modelBuilder.Entity<Map>().HasMany(m => m.Polygons)
                .WithRequired(p => p.Map);

            modelBuilder.Entity<Polygon>().HasKey(e => e.Id);
            modelBuilder.Entity<Polygon>().Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Path>().HasKey(e => e.Id);
            modelBuilder.Entity<Path>().Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Point>().HasKey(e => e.Id);
            modelBuilder.Entity<Point>().Property(e => e.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Point>().Property(p => p.X);
            modelBuilder.Entity<Point>().Property(p => p.X)
            .HasColumnAnnotation("Index",
            new IndexAnnotation(new IndexAttribute("IX_Points_X")));
            modelBuilder.Entity<Point>().Property(p => p.Y)
            .HasColumnAnnotation("Index",
            new IndexAnnotation(new IndexAttribute("IX_Points_Y")));
        }
    }
}