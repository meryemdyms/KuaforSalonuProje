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
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }

        
    }
}
