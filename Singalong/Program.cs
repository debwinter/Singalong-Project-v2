using MySql.Data.MySqlClient;
using System.Data;
using Singalong.Repositories;

namespace Singalong;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddScoped<IDbConnection>((s) =>
        {
            IDbConnection conn = new MySqlConnection(builder.Configuration.GetConnectionString("singalong"));
            conn.Open();
            return conn;
        });

        // need to talk about this ***
        builder.Services.AddScoped<SongRepo>();
        builder.Services.AddScoped<LyricsRepo>();

        //// may be able to comment this out ***
        //builder.Services.AddTransient<ISongRepository, SongRepo>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
