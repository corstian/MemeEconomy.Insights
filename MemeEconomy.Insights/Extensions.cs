using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace MemeEconomy.Insights
{
    public static class Extensions
    {
        public static string ToCursor(this Guid guid) => Convert.ToBase64String(guid.ToByteArray());
        public static Guid FromCursor(this string base64Guid) => new Guid(Convert.FromBase64String(base64Guid));

        public static IWebHost MigrateDatabase<TDbContext>(this IWebHost webHost)
            where TDbContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var db = services.GetRequiredService<TDbContext>();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            return webHost;
        }
    }
}
