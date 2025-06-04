using Microsoft.EntityFrameworkCore;
using NotificationHubHost.Data;
using NotificationHubHost.Hubs;

var builder = WebApplication.CreateBuilder(args);

// 1) DbContext + Migrations
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NotificationDB"))
);

// 2) SignalR
builder.Services.AddSignalR();

builder.Services.AddHostedService<ScheduledNotificationService>();

var app = builder.Build();

// Apply pending migrations at startup


// Map the Hub endpoint
app.MapHub<NotificationHub>("/Hubs/notificationHub");

app.Run();
