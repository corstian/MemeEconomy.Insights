using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Execution;
using GraphQL.Http;
using GraphQL.Server;
using GraphQL.Types;
using GraphQL.Types.Relay;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;
using System.Reflection;

namespace MemeEconomy.Insights
{
    public partial class Startup
    {
        public void ConfigureGraph(IServiceCollection services)
        {
            services.AddTransient(typeof(ConnectionType<>));
            services.AddTransient(typeof(EdgeType<>));
            services.AddTransient<PageInfoType>();
            services.AddSingleton<ULongGraphType>();

            Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(q => q.IsClass
                    && (q.Namespace?.StartsWith("MemeEconomy.Insights.Graph") ?? false))
                .ToList()
                .ForEach(type => services.AddSingleton(type));

            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddSingleton<IDependencyResolver>(q => new FuncDependencyResolver(q.GetRequiredService));
            services.TryAddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.TryAddSingleton<IDocumentWriter, DocumentWriter>();

            services.AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
            services.AddSingleton<IDocumentExecutionListener, DataLoaderDocumentListener>();

            services.AddGraphQL(options =>
            {
                options.ExposeExceptions = true;
            })
            .AddWebSockets();
        }
    }
}
