using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemeEconomy.Data
{
    public class MemeEconomyContext : DbContext
    {
        public MemeEconomyContext()
        {

        }

        public MemeEconomyContext(IConfiguration config)
        {

        }

        public DbSet<Investment> Investments { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }
    }
}
