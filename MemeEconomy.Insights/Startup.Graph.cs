using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Execution;
using GraphQL.Http;
using GraphQL.Server;
using GraphQL.Types;
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
            
        }
    }
}
