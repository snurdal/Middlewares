using Middlewares.Extensions;
using Middlewares.Handlers;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
//builder.Services.AddControllers(options =>
//{
//  options.Filters.Add<ValidateModelAttribute>(); // Add the custom model validation filter globally.
//});
var app = builder.Build();



// Configure the HTTP request pipeline for development.

if (!app.Environment.IsDevelopment())
{
    // Configure the HTTP request pipeline for production.
    app.UseSecureHeaders(); // Use the custom middleware to set secure headers.
    app.UseStrictTransportSecurity(); // Use the custom middleware to enforce HSTS (HTTP Strict Transport Security).
    app.UseCrossOriginPolicy(); // Use the custom middleware to set CORS (Cross-Origin Resource Sharing) policy.
    app.UseRequestSizelimit(1024 * 1024 * 10); // Set request size limit to 10MB.
    app.UserPermissionPolicy(); // Use the custom middleware to set permissions policy.
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>(); // Use the global exception handling middleware to catch and handle exceptions globally.

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS.

app.MapGet("/", () => "Hello World!");

app.Run();
