// Import the namespace that contains AppDbContext.
// AppDbContext is your Entity Framework Core database context class
// which acts as a bridge between your application and Azure SQL Database.
using Az_app_pritam.Data;

// Import Azure Identity library.
// Used when authenticating with Azure services using Managed Identity,
// Azure CLI login, Visual Studio login, etc.
// Currently not used in this code but may be used later.
using Azure.Identity;

// Import Entity Framework Core namespace.
// Provides DbContext, UseSqlServer(), migrations, LINQ support, etc.
using Microsoft.EntityFrameworkCore;

// Create a WebApplicationBuilder object.
// This is the starting point of the ASP.NET Core application.
// It loads configuration, services, logging, environment settings, etc.
var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------------------------
// SERVICE REGISTRATION SECTION
// Everything added here is registered with Dependency Injection (DI)
// Container and available throughout the application.
// --------------------------------------------------------------------

// Read the Azure SQL Database connection string from configuration.
//
// The value can come from:
// 1. appsettings.json
// 2. appsettings.Development.json
// 3. User Secrets
// 4. Environment Variables
// 5. Azure App Service Configuration
//
// Example:
// "ConnectionStrings": {
//    "AzureSQLDBConnstring": "..."
// }
var connectionString = builder.Configuration.GetConnectionString("AzureSQLDBConnstring");

// Register AppDbContext with Dependency Injection container.
//
// AddDbContext<T>() tells ASP.NET Core:
// "Whenever AppDbContext is requested, create and inject it."
//
// UseSqlServer() tells Entity Framework Core:
// "Use Microsoft SQL Server / Azure SQL Database as the database provider."
//
// EF Core will use this connection string whenever it needs
// to query or update the database.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register Razor Pages services.
//
// This enables:
// - Razor Pages routing
// - Page Models
// - Model Binding
// - Validation
// - Dependency Injection into pages
//
// Without this line, Razor Pages won't work.
builder.Services.AddRazorPages();

// Register Application Insights telemetry.
//
// Application Insights is Azure's monitoring solution.
//
// It automatically collects:
// - Request logs
// - Response times
// - Exceptions
// - Failed requests
// - Dependency calls
// - Performance metrics
//
// Data is sent to Azure Monitor.
builder.Services.AddApplicationInsightsTelemetry(
    new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
    {
        // Read Application Insights connection string
        // from configuration and connect the application
        // to the correct Azure monitoring resource.
        ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
    });

// --------------------------------------------------------------------
// APPLICATION BUILD SECTION
// Build the application using all registered services.
// After this point, the service collection becomes read-only.
// --------------------------------------------------------------------
var app = builder.Build();

// --------------------------------------------------------------------
// HTTP REQUEST PIPELINE CONFIGURATION
// Middleware executes in the order defined below.
// Every request passes through this pipeline.
// --------------------------------------------------------------------

// Check whether the application is running in Production.
//
// Development:
// ASPNETCORE_ENVIRONMENT=Development
//
// Production:
// ASPNETCORE_ENVIRONMENT=Production
if (!app.Environment.IsDevelopment())
{
    // Global exception handling middleware.
    //
    // If an unhandled exception occurs anywhere in the application,
    // users will be redirected to the Error page instead of seeing
    // a technical exception screen.
    app.UseExceptionHandler("/Error");

    // Enable HTTP Strict Transport Security (HSTS).
    //
    // Tells browsers:
    // "Always use HTTPS for this website."
    //
    // Helps protect against SSL stripping attacks.
    app.UseHsts();
}

// Automatically redirect HTTP requests to HTTPS.
//
// Example:
//
// http://mysite.com
//
// becomes
//
// https://mysite.com
app.UseHttpsRedirection();

// Enable ASP.NET Core routing system.
//
// Routing determines which page or endpoint should handle
// an incoming URL request.
app.UseRouting();

// Enable authorization middleware.
//
// Checks whether the current user has permission
// to access protected resources.
//
// Required when using:
// [Authorize]
//
// Even if authentication is not implemented yet,
// keeping this middleware is common practice.
app.UseAuthorization();

// Enable serving static assets.
//
// Static assets include:
// - CSS
// - JavaScript
// - Images
// - Fonts
//
// Example:
//
// /css/site.css
// /images/logo.png
app.MapStaticAssets();

// Map Razor Pages endpoints.
//
// This scans the Pages folder and automatically creates routes.
//
// Examples:
//
// Pages/Index.cshtml
//      -> /
//
// Pages/About.cshtml
//      -> /About
//
// Pages/Contact.cshtml
//      -> /Contact
app.MapRazorPages()

   // Enable static asset support for Razor Pages.
   .WithStaticAssets();

// Start the web application.
//
// Kestrel web server starts listening for requests.
//
// Local Example:
// https://localhost:5001
//
// Azure Example:
// https://yourapp.azurewebsites.net
app.Run();