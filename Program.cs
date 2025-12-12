using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using System.Security.Claims;
using System.Text;
using SacViet.Models;
using SacViet.Repositories;
using SacViet.Services;

// Register encoding provider for proper Vietnamese character support
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Persist Data Protection keys so auth cookies remain valid across restarts/instances
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "DataProtectionKeys")))
    .SetApplicationName("SacViet");

// Add session services for view count tracking
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add Entity Framework
builder.Services.AddDbContext<SacVietContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

// Configure Authentication
var authBuilder = builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;

    // Harden cookie settings
    options.Cookie.Name = ".SacViet.Auth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Add Google only when configured properly
var clientId = builder.Configuration["Authentication:Google:ClientId"];
var clientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
var hasGoogle = !string.IsNullOrWhiteSpace(clientId)
                && !string.IsNullOrWhiteSpace(clientSecret)
                && clientId != "YOUR_GOOGLE_CLIENT_ID_HERE"
                && clientSecret != "YOUR_GOOGLE_CLIENT_SECRET_HERE";

if (hasGoogle)
{
    authBuilder.AddGoogle(options =>
    {
        options.ClientId = clientId!;
        options.ClientSecret = clientSecret!;
        options.CallbackPath = "/signin-google";
        options.SaveTokens = true;
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        // Request additional scopes
        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
    });
}

// Register repositories
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register services
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IWriterService, WriterService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IWebCrawlerService, WebCrawlerService>();

// Register background services
builder.Services.AddHostedService<ScheduledPublishService>();

// Respect reverse proxy headers (needed on many hostings)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// Ensure DB index for GoogleID allows multiple NULLs
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<SacVietContext>();
    try
    {
        var sql = @"
DECLARE @userId INT = OBJECT_ID(N'dbo.Users');
IF @userId IS NOT NULL
BEGIN
    DECLARE @googleColId INT = (
        SELECT c.column_id FROM sys.columns c WHERE c.object_id = @userId AND c.name = 'GoogleID'
    );
    IF @googleColId IS NOT NULL
    BEGIN
        DECLARE @idxName NVARCHAR(128);
        SELECT TOP 1 @idxName = i.name
        FROM sys.indexes i
        JOIN sys.index_columns ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id
        WHERE i.object_id = @userId AND i.is_unique = 1 AND ic.column_id = @googleColId AND ISNULL(i.has_filter, 0) = 0;

        IF @idxName IS NOT NULL
        BEGIN
            EXEC('DROP INDEX [' + @idxName + '] ON [dbo].[Users]');
        END

        IF NOT EXISTS (
            SELECT 1 FROM sys.indexes WHERE name = 'UQ_Users_GoogleID_NotNull' AND object_id = @userId
        )
        BEGIN
            CREATE UNIQUE NONCLUSTERED INDEX [UQ_Users_GoogleID_NotNull]
            ON [dbo].[Users] ([GoogleID])
            WHERE ([GoogleID] IS NOT NULL);
        END
    END
END";
        ctx.Database.ExecuteSqlRaw(sql);
    }
    catch
    {
        // ignore startup index fix errors
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Must be before other middleware to correctly infer scheme behind proxy
app.UseForwardedHeaders();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add session middleware (must be before authentication)
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "article",
    pattern: "bai-viet/{slug}",
    defaults: new { controller = "Article", action = "Details" });

app.MapControllerRoute(
    name: "category",
    pattern: "category/{categorySlug:regex(^[a-z0-9-]+$)}",
    defaults: new { controller = "Category", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
