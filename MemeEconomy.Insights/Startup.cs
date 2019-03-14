using AspNetCoreRateLimit;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Execution;
using GraphQL.Http;
using GraphQL.Server;
using GraphQL.Types.Relay;
using MemeEconomy.Insights.Graph;
using MemeEconomy.Insights.Graph.Types;
using MemeEconomy.Insights.Models;
using MemeEconomy.Insights.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using RedditSharp;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data.SqlClient;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace MemeEconomy.Insights
{
    public partial class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddOptions();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();

            var reddit = 
                new Reddit(
                    new BotWebAgent(
                        _config["reddit:username"],
                        _config["reddit:password"],
                        _config["reddit:clientId"],
                        _config["reddit:clientSecret"],
                        _config["reddit:redirectUri"]),
                    false);

            services.AddSingleton((serviceProvider) =>
            {
                var connection = new SqlConnection(_config["database:connection"]);
                var compiler = new SqlServerCompiler();
                return new QueryFactory(connection, compiler);
            });

            services.AddSingleton(reddit);

            services.Configure<IpRateLimitOptions>(_config.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(_config.GetSection("IpRateLimitPolicies"));

            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddDbContext<MemeEconomyContext>
                (options => options.UseSqlServer(_config["database:connection"]));

            services.AddScoped<MemeEconomyContext>();
            services.AddSingleton<IContextProvider<MemeEconomyContext>, ContextProvider<MemeEconomyContext>>();

            ConfigureGraph(services);

            // Registration of GraphQL dependencies
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
            services.TryAddSingleton<IDocumentExecutionListener, DataLoaderDocumentListener>();
            services.TryAddSingleton<IDependencyResolver>(q => new FuncDependencyResolver(q.GetRequiredService));
            services.TryAddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.TryAddSingleton<IDocumentWriter, DocumentWriter>();

            // Registration for Relay related types
            services.AddTransient(typeof(ConnectionType<>));
            services.AddTransient(typeof(EdgeType<>));
            services.AddTransient<PageInfoType>();

            services.AddSingleton<GraphSchema>();
            services.AddSingleton<GraphQuery>();
            services.AddSingleton<GraphSubscription>();
            services.AddSingleton<InvestmentType>();
            services.AddSingleton<OpportunityType>();
            services.AddSingleton<OpportunityOrderType>();

            services
                .AddGraphQL(options =>
                {
                    options.ExposeExceptions = true;
                })
                .AddWebSockets();

            services.AddSingleton<IHostedService, MemeEconomyStalker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseIpRateLimiting();

            app.UseWebSockets();

            app.UseGraphQL<GraphSchema>("/graph");
            app.UseGraphQLWebSockets<GraphSchema>("/graph");
        }
    }
}
