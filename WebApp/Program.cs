using WebApp.Models;
using WebApp.Hubs;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.CodeAnalysis.Differencing;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WebAppContextConnection") ?? throw new InvalidOperationException("Connection string 'WebAppContextConnection' not found.");

builder.Services.AddDbContext<WebAppContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<WebAppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<WebAppContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

//Gamemanager
builder.Services.AddSingleton<GameManager>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});

//Recaptcha
builder.Services.AddHttpClient<ReCaptcha>(client =>
{
    client.BaseAddress = new Uri("https://www.google.com/recaptcha/api/siteverify");
});

//Builder
var app = builder.Build();

// CSP && headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy",
        "default-src 'self' wss: ws:;" +
        "script-src 'self' 'unsafe-inline';" +
        "style-src 'self' 'unsafe-inline'" +
        "font-src 'self'" +
        "img-src 'self' data:;"
    );
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.Remove("X-Powered-By");
    await next();
});


app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseWebSockets();

app.UseRateLimiter();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Battleship}/{action=Index}/{id?}");

app.MapHub<BattleShipHub>("/BattleShipHub");

app.MapRazorPages();

app.Run();