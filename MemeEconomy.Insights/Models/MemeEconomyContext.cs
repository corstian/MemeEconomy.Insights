using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace MemeEconomy.Insights.Models
{
    public class MemeEconomyContext : DbContext
    {
        private string _connectionString;

        [Obsolete("This constructor is only used for running migrations!")]
        public MemeEconomyContext()
        {
            _connectionString = "Data Source=(localdb)\\.;Initial Catalog=memeeconomy;Integrated Security=True;";
        }

        private MemeEconomyContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MemeEconomyContext(IConfiguration config) : this(config["database:connection"]) { }
        public MemeEconomyContext(DbContextOptions<MemeEconomyContext> options) : base(options) { }


        public DbSet<Investment> Investments { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString, q => q.EnableRetryOnFailure());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Opportunity>()
                .HasAlternateKey(c => c.PostId);
        }
    }
}
