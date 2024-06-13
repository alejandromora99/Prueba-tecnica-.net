using CronProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CronProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<DataRecord> DataRecords { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DataRecord>(entity =>
            {
                entity.Property(e => e.RutClienteEnvia).IsRequired().HasMaxLength(12);
                entity.Property(e => e.NombreClienteEnvia).IsRequired().HasMaxLength(100);
                entity.Property(e => e.IdTransaccion).IsRequired().HasMaxLength(20);
                entity.Property(e => e.RutClienteRecibe).IsRequired().HasMaxLength(12);
                entity.Property(e => e.NombreClienteRecibe).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CodigoBancoReceptor).IsRequired().HasMaxLength(5);
                entity.Property(e => e.NombreBancoReceptor).IsRequired().HasMaxLength(100);
                entity.Property(e => e.MontoTransferencia).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.MonedaTransferencia).IsRequired().HasMaxLength(3);
                entity.Property(e => e.FechaTransferencia).IsRequired().HasColumnType("date");
            });
        }
    }
}
