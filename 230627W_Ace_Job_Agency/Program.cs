using _230627W_Ace_Job_Agency.Middleware;
using _230627W_Ace_Job_Agency.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.AllowedForNewUsers = true;
}).AddSignInManager().AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(Config => {
    Config.LoginPath = "/Login";
    Config.ExpireTimeSpan = TimeSpan.FromSeconds(30);
    Config.SlidingExpiration = true;
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options=> {
    options.IdleTimeout = TimeSpan.FromSeconds(25);
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequiredLength = 12;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions {
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "wwwroot/uploads")),
    RequestPath = "/uploads"
});

app.UseSession();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseMiddleware<SessionTimeoutMiddleware>();

app.UseAntiforgery();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
