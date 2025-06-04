using Microsoft.EntityFrameworkCore;
using RmoSignalrPoC.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Register the SignalR client service to connect to NotificationHubHost.
// The URL for the hub is configured via "NotificationHubUrl" in configuration.
builder.Services.AddSingleton<NotificationClient>();

// MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

// NOTE: Hub endpoint mapping removed to avoid hosting our own hub endpoint
// app.MapHub<NotificationHub>("/Hubs/NotificationHub");

// MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Notification}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
