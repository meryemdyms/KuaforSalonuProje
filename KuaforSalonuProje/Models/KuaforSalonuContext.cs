using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KuaforSalonuProje.Models
{
    public class KuaforSalonuContext : IdentityDbContext<ApplicationUser>
    {
        public KuaforSalonuContext(DbContextOptions<KuaforSalonuContext> options) : base(options) { }

        public DbSet<Salon> Salonlar { get; set; }
        public DbSet<Calisan> Calisanlar { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<Kullanici> Kullanıcılar { get; set; }
        public DbSet<Hizmetler> Hizmetler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Randevu ve Çalışan ilişkisi
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Calisan)
                .WithMany(c => c.Randevular)
                .HasForeignKey(r => r.CalisanId)
                .OnDelete(DeleteBehavior.Restrict);

            // Randevu ve Salon ilişkisi
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Salon)
                .WithMany(s => s.Randevular)
                .HasForeignKey(r => r.SalonId)
                .OnDelete(DeleteBehavior.Restrict);

            // Hizmetler ve Salon ilişkisi
            modelBuilder.Entity<Hizmetler>()
                .HasOne(h => h.Salon)
                .WithMany(s => s.Hizmetler)
                .HasForeignKey(h => h.SalonId);
        }
    }
}