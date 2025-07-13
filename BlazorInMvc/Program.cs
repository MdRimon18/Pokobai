using Domain.Data.Repository;
using Domain.DbContex;
using Domain.Services;
using Domain.Services.Accounts;
using Domain.Services.Inventory;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// Enable detailed exceptions for development
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
 
builder.Services.Configure<CircuitOptions>(options =>
{
    options.DetailedErrors = true;
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Add this line to register Razor Pages services
//builder.Services.AddServerSideBlazor();
builder.Services.AddMemoryCache();
builder.Services.AddApplicationServices();
// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});


// Ensure the required NuGet package is installed:
// Install-Package Swashbuckle.AspNetCore


var app = builder.Build();

// ✅ Enable Swagger for both Development and Production
app.UseSwagger();
app.UseSwaggerUI(c =>
{
   c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
   c.RoutePrefix = "swagger"; // Optional; change to "" to set Swagger as home page
 //   c.RoutePrefix =String.Empty;
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
                    // Generates Swagger JSON
   
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
// Map Razor Pages
app.MapRazorPages(); // Add this line to enable Razor Pages routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=EcomProducts}/{action=Index}/{id?}");

// Map Blazor Hub for server-side Blazor
//app.MapBlazorHub();

// Map fallback to the _Host.cshtml for Blazor
//app.MapFallbackToPage("/_Host");





app.Run();
