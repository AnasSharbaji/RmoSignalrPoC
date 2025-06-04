//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;
//using System.IO;

//namespace NotificationHubHost.Data
//{
//    /// <summary>
//    /// Stellt den DbContext für EF Core Design-Time Tools zur Verfügung.
//    /// </summary>
//    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
//    {
//        public AppDbContext CreateDbContext(string[] args)
//        {
//            // Konfiguration aus appsettings.json laden
//            var config = new ConfigurationBuilder()
//                .SetBasePath(Directory.GetCurrentDirectory())           // Projektordner
//                .AddJsonFile("appsettings.json", optional: false)      // stelle sicher, dass Copy to Output eingestellt ist
//                .Build();

//            // Optionen für DbContextBuilder setzen
//            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
//            var connString = config.GetConnectionString("NotificationDB");
//            optionsBuilder.UseSqlServer(connString);

//            return new AppDbContext(optionsBuilder.Options);
//        }
//    }
//}
