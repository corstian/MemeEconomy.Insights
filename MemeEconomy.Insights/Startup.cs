using AspNetCoreRateLimit;
using GraphQL.Server;
using MemeEconomy.Insights.Graph;
using MemeEconomy.Insights.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            services.Configure<IpRateLimitOptions>(_config.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(_config.GetSection("IpRateLimitPolicies"));

            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            ConfigureData(services);
            ConfigureGraph(services);

            // In order to prevent duplicate (And therefore incorrect) data showing up
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
