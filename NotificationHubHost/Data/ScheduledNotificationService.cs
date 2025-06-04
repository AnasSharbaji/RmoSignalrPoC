using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationHubHost.Hubs;
using NotificationHubHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotificationHubHost.Data
{

    public class ScheduledNotificationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<NotificationHub> _hubContext;

        // Set your desired times (e.g., 08:00 and 20:00)
        private readonly TimeSpan[] _sendTimes = new[]
        {
            new TimeSpan(8, 0, 0),
            new TimeSpan(18, 0, 0)
        };

        public ScheduledNotificationService(IServiceProvider serviceProvider, IHubContext<NotificationHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextSend = _sendTimes
                    .Select(t => now.Date.Add(t) > now ? now.Date.Add(t) : now.Date.AddDays(1).Add(t))
                    .OrderBy(t => t)
                    .First();

                var delay = nextSend - now;
                if (delay.TotalMilliseconds > 0)
                    await Task.Delay(delay, cancellationToken);

                // Send notifications
                await SendLatestNotifications();

                // Wait a minute to avoid double sending if the loop is too fast
                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }

        private async Task SendLatestNotifications()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Get the latest notifications (e.g., last 5)
                var notifications = dbContext.Notifications
                    .OrderByDescending(n => n.Timestamp)
                    .Take(5)
                    .Select(n => new NotificationDto
                    {
                        User = n.User,
                        Text = n.Text,
                        Category = n.Category,
                        Timestamp = n.Timestamp
                    })
                    .OrderBy(n => n.Timestamp)
                    .ToList();

                // Broadcast to all clients
                foreach (var n in notifications)
                {
                    await _hubContext.Clients.All.SendAsync(
                        "ReceiveNotification",
                        n.User,
                        n.Text,
                        n.Category,
                        n.Timestamp  // Add the timestamp parameter to match client's handler signature
                    );
                }
            }
        }
    }
}


