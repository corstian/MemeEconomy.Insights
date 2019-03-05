using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace MemeEconomy.Data
{
    public class MemeEconomyContext : DbContext
    {
        private string _connectionString;

        [Obsolete("This constructor is only used for running migrations!")]
        public MemeEconomyContext()
        {
            _connectionString = "Data Source=(localdb)\\.;Initial Catalog=skyhop_hosting;Integrated Security=True;";
        }

        private MemeEconomyContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MemeEconomyContext(IConfiguration config) : this(config["ConnectionStrings:DefaultConnection"]) { }
        public MemeEconomyContext(DbContextOptions<MemeEconomyContext> options) : base(options) { }


        public DbSet<Investment> Investments { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Opportunity>()
                .HasAlternateKey(c => c.PostId);
        }
    }
}
