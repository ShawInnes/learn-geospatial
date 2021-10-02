using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace api.Data
{
    public partial class PostGisDbContext : DbContext
    {
        public PostGisDbContext()
        {
        }

        public PostGisDbContext(DbContextOptions<PostGisDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PublicArt> PublicArts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("fuzzystrmatch")
                .HasPostgresExtension("h3")
                .HasPostgresExtension("postgis")
                .HasPostgresExtension("postgis_tiger_geocoder")
                .HasPostgresExtension("postgis_topology")
                .HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<PublicArt>(entity =>
            {
                entity.HasKey(e => e.Gid)
                    .HasName("public_art_pkey");

                entity.ToTable("public_art");

                entity.HasIndex(e => e.TheGeom, "public_art_the_geom_gist")
                    .HasMethod("gist");

                entity.Property(e => e.Gid).HasColumnName("gid");

                entity.Property(e => e.Artist)
                    .HasMaxLength(50)
                    .HasColumnName("artist");

                entity.Property(e => e.Description)
                    .HasMaxLength(600)
                    .HasColumnName("description");

                entity.Property(e => e.Latitude).HasColumnName("latitude");

                entity.Property(e => e.Location)
                    .HasMaxLength(100)
                    .HasColumnName("location");

                entity.Property(e => e.Longitude).HasColumnName("longitude");

                entity.Property(e => e.Material)
                    .HasMaxLength(150)
                    .HasColumnName("material");

                entity.Property(e => e.TheGeom).HasColumnName("the_geom");

                entity.Property(e => e.Title)
                    .HasMaxLength(150)
                    .HasColumnName("title");

                entity.Property(e => e.Year).HasColumnName("year");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
