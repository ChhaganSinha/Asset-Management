using AssetManagement.DataContext;
using AssetManagement.DataContext.Models;
using AssetManagement.Dto;
using AssetManagement.Repositories;
using AssetManagement.Server;
using AssetManagement.Server.EmailService;
using AssetManagement.Server.Intrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using AssetManagement.Server.Service;

var builder = WebApplication.CreateBuilder(args);

// Add environment variables
builder.Configuration.AddEnvironmentVariables(prefix: "ASPNETCORE_");

// Add appsettings.json files
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

var config = builder.Configuration;

// Configure Serilog
builder.Services.AddLogging((builder) =>
{
    Serilog.Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateLogger();
    builder.AddSerilog();
});


// Configure CORS to allow multiple domains
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policyBuilder =>
        policyBuilder.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowCredentials()
            .AllowAnyMethod());
});

// Add controllers with OData and NewtonsoftJson support
builder.Services.AddControllers()
    .AddODataControllers()
    .AddNewtonsoftJson();

// Add background service options
builder.Services.Configure<BackgroundServiceOptions>(config.GetSection("BackgroundService"));

// Add hosted service
builder.Services.AddHostedService<TimedHostedService>();

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Configure database connections
var useSqlLite = builder.Configuration.GetValue<bool>("UseSqlLite");
var sqlLiteAssetAuthConn = builder.Configuration.GetValue<string>("SqlLiteAuthConnectionString");
var sqlLiteAssetConn = builder.Configuration.GetValue<string>("SqlLiteAssetConnectionString");

var conStringAuth = builder.Configuration.GetValue<string>("ConnectionStrings:MySqlAuthDbConnection");
var conString = builder.Configuration.GetValue<string>("ConnectionStrings:MySqlConnection");

if (useSqlLite)
{
    builder.Services.AddDbContext<AuthDbContext>(options =>
        options.UseSqlite(sqlLiteAssetAuthConn));

    builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseSqlite(sqlLiteAssetConn));
}
else
{
    //builder.Services.AddDbContext<AuthDbContext>(options =>
    //{
    //    options.UseMySql(conStringAuth, ServerVersion.AutoDetect(conStringAuth));
    //});
    //builder.Services.AddDbContext<AppDbContext>(options =>
    //{
    //    options.UseMySql(conString, ServerVersion.AutoDetect(conString));
    //});
}

// Add Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

// Configure Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+";
    options.User.RequireUniqueEmail = true;
});

// Configure application cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // <-- important for HTTPS
    options.Cookie.SameSite = SameSiteMode.None;
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

// Add logging
builder.Services.AddLogging();
builder.Services.AddScoped<BaseRepository>();
builder.Services.AddScoped<IAppRepository, AppRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IAllocationRepository, AllocationRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();

builder.Services.AddSingleton(builder.Configuration.GetSection("MailSettings").Get<MailSettings>());
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<SharePointService>();
builder.Services.AddScoped<OnboardingConfirmation>();
// Add controllers and Razor Pages
builder.Services.AddControllers();
builder.Services.AddRazorPages();

// Add logging
builder.Logging.AddDebug();
builder.Logging.AddConsole();

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

var defaultRoles = builder.Configuration.GetSection("DefaultRoles").Get<string[]>() ?? new string[] { "SuperAdmin", "Admin" };
var defaultUsername = builder.Configuration.GetValue<string>("DefaultUser:Username") ?? "DefaultUser";
var defaultUserId = builder.Configuration.GetValue<string>("DefaultUser:UserId") ?? "default@assetmanagementapp.com";
var defaultPassword = builder.Configuration.GetValue<string>("DefaultUser:Password") ?? "Asset@2023";

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AuthDbContext>().Database.Migrate();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (db.Database.EnsureCreated())
    {
        // seed data if a single tenant application
    }
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    
    //// Check if the default admin user exists
    //var adminUser = await userManager.FindByNameAsync("Admin");
    //if (adminUser == null)
    //{
    //    adminUser = new ApplicationUser
    //    {
    //        UserName = defaultUsername,
    //        Email = defaultUserId
    //        // Set other properties for the admin user as needed
    //    };
    //    var result = await userManager.CreateAsync(adminUser, defaultPassword);
    //    if (result.Succeeded)
    //    {
    //        foreach (var roleName in defaultRoles)
    //        {
    //            var roleExists = await roleManager.RoleExistsAsync(roleName);
    //            if (!roleExists)
    //            {
    //                var role = new IdentityRole<Guid>(roleName);
    //                await roleManager.CreateAsync(role);
    //            }
    //        }
    //        // Create the "Admin" role if it doesn't exist
    //        var adminRole = new IdentityRole<Guid>("SuperAdmin");
    //        var createRoleResult = await roleManager.CreateAsync(adminRole);

    //        // Assign the admin user to the "Admin" role
    //        var addUserRoleResult = await userManager.AddToRoleAsync(adminUser, "SuperAdmin");
    //    }
    //}
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

