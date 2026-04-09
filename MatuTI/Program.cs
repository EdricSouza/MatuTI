using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MatuTI.Data;
using MatuTI.Models;

var builder = WebApplication.CreateBuilder(args);

//
// ================= DATABASE =================
//
var databasePath = Environment.GetEnvironmentVariable("DB_PATH") ?? "matuti.db";
var databaseDirectory = Path.GetDirectoryName(databasePath);

if (!string.IsNullOrWhiteSpace(databaseDirectory))
{
    Directory.CreateDirectory(databaseDirectory);
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={databasePath}"));

//
// ================= IDENTITY =================
//
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AcessoNegado";
});

//
// ================= AUTHORIZATION =================
//
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",
        policy => policy.RequireRole("Admin"));
});

//
// ================= RAZOR PAGES =================
//
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Empresas", "Admin");
    options.Conventions.AuthorizeFolder("/Questoes", "Admin");
    options.Conventions.AuthorizeFolder("/Avaliacao");
    options.Conventions.AuthorizePage("/Index");
});

//
// ================= PORTA (RENDER / DOCKER) =================
//
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();

//
// ================= PIPELINE =================
//
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

//
// ================= MIGRATION + SEED =================
//
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var db = services.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    await Seed.InicializarAsync(services);
}

app.Run();