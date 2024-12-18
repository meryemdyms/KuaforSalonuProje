using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KuaforSalonuProje.Models; // ApplicationUser ve KuaforSalonuContext'inizi içeren namespace

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // PostgreSQL veritabanı bağlantısı
        builder.Services.AddDbContext<KuaforSalonuContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Identity service
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<KuaforSalonuContext>()
            .AddDefaultTokenProviders();

        // Authorization politikaları
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            options.AddPolicy("AnyUser", policy => policy.RequireRole("Admin", "User"));
        });

        // MVC servislerini ekle
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Hata yönetimi ve güvenlik yapılandırması
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts(); // HTTP Strict Transport Security (HSTS)
        }

        // HTTPS yönlendirme, statik dosyalar ve routing işlemleri
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        // Routing ve authorization işlemleri
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        // Controller route'larını yapılandırma
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // Veritabanı ve rollerin başlatılması
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            RoleInitializer.SeedRolesAsync(roleManager).Wait();
        }

        // Uygulamanın çalıştırılması
        app.Run();
    }
}
