using lecheriaSC.Core.Entidades;
using Microsoft.EntityFrameworkCore;

namespace lecheriaSC.Infrastructure.Data
{
    public class SucursalContext : DbContext
    {
        public SucursalContext(DbContextOptions<SucursalContext> options) : base(options) { }

        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Reporte> Reportes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Sucursal
            modelBuilder.Entity<Sucursal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Codigo).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("NOW()");
            });

            // Solicitud
            modelBuilder.Entity<Solicitud>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CodigoSucursal).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Tipo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Departamento).IsRequired().HasMaxLength(50);
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("NOW()");
            });

            // Inventario
            modelBuilder.Entity<Inventario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CodigoSucursal).IsRequired().HasMaxLength(20);
                entity.Property(e => e.NombreProducto).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UltimaActualizacion).HasDefaultValueSql("NOW()");
            });

            // Reporte
            modelBuilder.Entity<Reporte>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CodigoSucursal).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Tipo).IsRequired().HasMaxLength(20);
            });
        }
    }
}
