using System.Reactive.Subjects;
using MemeEconomy.Insights.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MemeEconomy.Insights
{
    public class Program
    {
        public static readonly ISubject<Opportunity> Opportunities = new Subject<Opportunity>();
        public static readonly ISubject<Investment> Investments = new Subject<Investment>();
        
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .MigrateDatabase<MemeEconomyContext>()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
