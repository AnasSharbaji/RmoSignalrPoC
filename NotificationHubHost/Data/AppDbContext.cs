using Microsoft.EntityFrameworkCore;
using NotificationHubHost.Models;

namespace NotificationHubHost.Data
{
    /// <summary>
    /// EF-Core DbContext zum Speichern von MessageEntity.
    /// </summary>

    public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public DbSet<NotificationEntity> Notifications { get; set; }
        }
    

}
