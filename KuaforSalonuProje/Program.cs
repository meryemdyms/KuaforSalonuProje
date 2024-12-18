using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KuaforSalonuProje.Models; // ApplicationUser ve KuaforSalonuContext'inizi i�eren namespace

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // PostgreSQL veritaban� ba�lant�s�
        builder.Services.AddDbContext<KuaforSalonuContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Identity service
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<KuaforSalonuContext>()
            .AddDefaultTokenProviders();

        // Authorization politikalar�
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            options.AddPolicy("AnyUser", policy => policy.RequireRole("Admin", "User"));
        });

        // MVC servislerini ekle
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Hata y�netimi ve g�venlik yap�land�rmas�
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts(); // HTTP Strict Transport Security (HSTS)
        }

        // HTTPS y�nlendirme, statik dosyalar ve routing i�lemleri
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        // Routing ve authorization i�lemleri
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        // Controller route'lar�n� yap�land�rma
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // Veritaban� ve rollerin ba�lat�lmas�
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            /*RoleInitializer.SeedRolesAsync(roleManager).Wait();*/
        }

        // Uygulaman�n �al��t�r�lmas�
        app.Run();
    }
}
