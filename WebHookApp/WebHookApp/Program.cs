using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebHookApp;
using WebHookApp.Hubs;
using WebHookApp.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllers();
        builder.Services.AddRazorPages();
        builder.Services.AddSignalR();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddSwaggerGen();

    var connectionString = builder.Configuration.GetConnectionString("DBConn");

        builder.Services.AddDbContext<webHookDataContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        builder.Services.AddScoped<IWebHookService, WebHookServices>();

        // Build the application

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebHook API v1");
                c.RoutePrefix = "api-docs";
            });
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
       

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapRazorPages();
            endpoints.MapHub<webHookHub>("/webhook");
            endpoints.MapFallbackToFile("index.html");       // Serve Blazor WebAssembly from index.html
        });

        app.Run();
    }
}