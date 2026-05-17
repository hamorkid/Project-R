using Microsoft.EntityFrameworkCore;
using Project_R_._8.Services;
using System;

// Add this line in Program.cs before building the app
AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "APP_DATA"));

System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
{
    FileName = "sqllocaldb",
    Arguments = "start MSSQLLocalDB",
    CreateNoWindow = true,
    UseShellExecute = false
})?.WaitForExit(); // Waits for this to finish before continuing

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddScoped<DBHelper>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


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

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

lifetime.ApplicationStopping.Register(() =>
{
    Console.WriteLine("APP STOPPING - Stopping LocalDB...");
    try
    {
        var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = "sqllocaldb",
            Arguments = "stop MSSQLLocalDB",
            CreateNoWindow = true,
            UseShellExecute = false
        });
        process?.WaitForExit();
        Console.WriteLine("LOCALDB STOPPED SUCCESSFULLY");
    }
    catch (Exception ex)
    {
        Console.WriteLine("FAILED TO STOP LOCALDB: " + ex.Message);
    }
});

app.Run();
