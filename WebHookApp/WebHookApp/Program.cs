using Microsoft.EntityFrameworkCore;
using WebHookApp.Services;
using WebHookApp.Hubs;
using Microsoft.AspNetCore.Builder;

namespace WebHookApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DbConn");

            // Configure the DbContext with MySQL
            builder.Services.AddDbContext<webHookDataContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            // Register your services for dependency injection
            builder.Services.AddScoped<IWebHookService, WebHookServices>();

            builder.Services.AddControllers();

            builder.Services.AddServerSideBlazor();

            // Add SignalR services
            builder.Services.AddSignalR();
            builder.Services.AddRazorPages();

            builder.Services.AddSwaggerGen();

            // Build the application
            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging(); // Use for debugging in development mode
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebHook API v1");
                    c.RoutePrefix = string.Empty; 
                });
            }
            else
            {
                app.UseExceptionHandler("/Error"); // Handle errors in production
                app.UseHsts(); // Enable HTTP Strict Transport Security
            }

            app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
            app.UseStaticFiles(); // Serve static files

            // Configure routing
            app.MapRazorPages(); // Map Razor Pages
            app.MapControllers(); // Map API controllers if you have them
            app.MapHub<webHookHub>("/webhook"); // Map your SignalR hub

            app.Run(); // Start the application
        }
    }
}
