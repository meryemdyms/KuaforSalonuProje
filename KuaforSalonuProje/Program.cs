using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KuaforSalonuProje.Models; // ApplicationUser ve KuaforSalonuContext'inizi içeren namespace

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL veritabaný baðlantýsý
builder.Services.AddDbContext<KuaforSalonuContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity kurulumu (kullanýcý yönetimi için)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<KuaforSalonuContext>() // Doðru veritabaný baðlantýsý
    .AddDefaultTokenProviders();

// Authorization politikalarý
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
    options.AddPolicy("AnyUser", policy => policy.RequireRole("Admin", "User"));
});

// MVC servislerini ekle
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Hata yönetimi ve güvenlik yapýlandýrmasý
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HTTP Strict Transport Security (HSTS)
}

// HTTPS yönlendirme, statik dosyalar ve routing iþlemleri
app.UseHttpsRedirection();
app.UseStaticFiles();

// Routing ve authorization iþlemleri
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Controller route'larýný yapýlandýrma
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Uygulamanýn çalýþtýrýlmasý
app.Run();
