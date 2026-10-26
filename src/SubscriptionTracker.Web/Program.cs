
using Microsoft.EntityFrameworkCore;
using SubscriptionTracker.Web.Data;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Veri işlemlerinde kültür tutarlılığı
CultureInfo.DefaultThreadCurrentCulture =
    CultureInfo.InvariantCulture;

CultureInfo.DefaultThreadCurrentUICulture =
    CultureInfo.InvariantCulture;

// Veritabanı bağlantısı
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "DefaultConnection bağlantı bilgisi bulunamadı."
    );

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity yapılandırması
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Giriş ve yetkilendirme yönlendirmeleri
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// MVC servisleri
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Üretim ortamında kullanıcıya teknik hata detayları gösterilmez
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 404 ve diğer HTTP hata kodları için
app.UseStatusCodePagesWithReExecute(
    "/Home/Error",
    "?statusCode={0}"
);

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
)
.WithStaticAssets();

app.Run();
