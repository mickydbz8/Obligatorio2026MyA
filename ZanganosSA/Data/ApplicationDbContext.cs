using Microsoft.EntityFrameworkCore;
using ZanganosSA.Models;

namespace ZanganosSA.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Apiario> Apiarios { get; set; }
        public DbSet<Colmena> Colmenas { get; set; }
        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<Cosecha> Cosechas { get; set; }
        public DbSet<Barril> Barriles { get; set; }
        public DbSet<ColmenaCosecha> ColmenaCosechas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar relación N a M entre Colmena y Cosecha
            modelBuilder.Entity<ColmenaCosecha>()
                .HasKey(cc => new { cc.ColmenaId, cc.CosechaId });

            modelBuilder.Entity<ColmenaCosecha>()
                .HasOne(cc => cc.Colmena)
                .WithMany(c => c.ColmenaCosechas)
                .HasForeignKey(cc => cc.ColmenaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ColmenaCosecha>()
                .HasOne(cc => cc.Cosecha)
                .WithMany(c => c.ColmenaCosechas)
                .HasForeignKey(cc => cc.CosechaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
