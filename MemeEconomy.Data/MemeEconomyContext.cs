using Microsoft.EntityFrameworkCore;

namespace MemeEconomy.Data
{
    public class MemeEconomyContext : DbContext
    {
        public DbSet<Investment> Investments { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }
    }
}
