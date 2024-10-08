using ELearningApp.Core.Models;
using ELearningApp.Service.DB;
using ELearningApp.Service.DB.DataHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure services
ConfigureServices(builder);

// Build the app
var app = builder.Build();

// Configure middleware
ConfigureMiddleware(app);

// Run the application
app.Run();

// Method to configure services
void ConfigureServices(WebApplicationBuilder builder)
{
    // Configure database connection
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    // Add DbContext
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    // Add developer exception page for database-related errors
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    // Configure Identity services
    builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        options.SignIn.RequireConfirmedAccount = false)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

    // Add MVC controllers with views
    builder.Services.AddControllersWithViews();

    // Register DataHelper
    builder.Services.AddScoped(typeof(IDataHelper<>), typeof(DataHelper<>));
}

// Method to configure middleware
void ConfigureMiddleware(WebApplication app)
{
    // Configure error handling and HSTS for different environments
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint(); // For EF migrations
    }
    else
    {
        app.UseExceptionHandler("/Home/Error"); // Global error handler
        app.UseHsts(); // Use HSTS for secure connections
    }

    // Configure HTTPS redirection and static files
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    // Configure routing
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    // Define the default route
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Map Razor Pages
    app.MapRazorPages();
}
