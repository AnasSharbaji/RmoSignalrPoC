using Microsoft.AspNetCore.SignalR;
using NotificationHubHost.Data;
using NotificationHubHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationHubHost.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly AppDbContext _dbContext;

        public NotificationHub(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task BroadcastNotification(string user, string text, string category)
        {
            var entity = new NotificationEntity
            {
                User = user,
                Text = text,
                Category = category,
                Timestamp = DateTime.UtcNow
            };
            _dbContext.Notifications.Add(entity);
            await _dbContext.SaveChangesAsync();

            await Clients.All.SendAsync(
                "ReceiveNotification",
                entity.User,
                entity.Text,
                entity.Category,
                entity.Timestamp
            );
        }

        public Task<List<NotificationDto>> GetRecentNotifications(int count)
        {
            var list = _dbContext.Notifications
                .OrderByDescending(n => n.Timestamp)
                .Take(count)
                .Select(n => new NotificationDto
                {
                    User = n.User,
                    Text = n.Text,
                    Category = n.Category,
                    Timestamp = n.Timestamp
                })
                .OrderBy(n => n.Timestamp)
                .ToList();

            return Task.FromResult(list);
        }

        public override async Task OnConnectedAsync()
        {
            // Hole die letzten 5 Benachrichtigungen
            var recentNotifications = await GetRecentNotifications(5);

            // Sende sie an den verbundenen Client
            foreach (var notification in recentNotifications)
            {
                await Clients.Caller.SendAsync(
                    "ReceiveNotification",
                    notification.User,
                    notification.Text,
                    notification.Category,
                    notification.Timestamp  // Add the timestamp parameter to match client's handler signature
                );
            }

            await base.OnConnectedAsync();
        }
    }
}
