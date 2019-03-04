using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Execution;
using GraphQL.Http;
using GraphQL.Server;
using GraphQL.Types.Relay;
using MemeEconomy.Insights.Graph;
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
            // Registration of GraphQL dependencies
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddSingleton<IDependencyResolver>(q => new FuncDependencyResolver(q.GetRequiredService));
            services.TryAddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.TryAddSingleton<IDocumentWriter, DocumentWriter>();
            services.AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
            services.AddSingleton<IDocumentExecutionListener, DataLoaderDocumentListener>();

            // Registration for Relay related types
            services.AddTransient(typeof(ConnectionType<>));
            services.AddTransient(typeof(EdgeType<>));
            services.AddTransient<PageInfoType>();

            // Root registrations for our schema
            services.AddSingleton<Schema>();
            services.AddSingleton<Query>();
            services.AddSingleton<Subscription>();

            /*
             * Automagically register GraphQL types.
             * 
             * We're using reflection in order to grab all classes in the 
             * *.Types namespace. We depend on the assumption that only Graph
             * related types will be created in this folder and that all
             * classes have a constructor which can be resolved by the DI container.
             */
            Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(q => q.IsClass
                    && q.Namespace.StartsWith("MemeEconomy.Insights.Graph.Types"))
                .ToList()
                .ForEach(type => services.AddSingleton(type));

            services
                .AddGraphQL()
                .AddWebSockets();
        }
    }
}
