using MemeEconomy.Insights.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RedditSharp;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data.SqlClient;

namespace MemeEconomy.Insights
{
    public partial class Startup
    {
        private void ConfigureData(IServiceCollection services)
        {
            services.AddScoped<MemeEconomyContext>();
            services.AddSingleton<IContextProvider<MemeEconomyContext>, ContextProvider<MemeEconomyContext>>();

            services.AddDbContext<MemeEconomyContext>
                (options => options.UseSqlServer(_config["database:connection"]));

            services.AddSingleton((serviceProvider) =>
            {
                var connection = new SqlConnection(_config["database:connection"]);
                var compiler = new SqlServerCompiler();
                return new QueryFactory(connection, compiler);
            });

            var reddit =
                    new Reddit(
                        new BotWebAgent(
                            _config["reddit:username"],
                            _config["reddit:password"],
                            _config["reddit:clientId"],
                            _config["reddit:clientSecret"],
                            _config["reddit:redirectUri"]),
                        false);

            services.AddSingleton(reddit);
        }
    }
}
