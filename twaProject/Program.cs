using Microsoft.EntityFrameworkCore;
using twaProject.Classes;
using twaProject.Components;

namespace twaProject;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        //Injecting dependencies
        builder.Services.AddSingleton<StateManager>();
        builder.Services.AddDbContext<MainDbContext>(options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WebDb;Trusted_Connection=True;"));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}