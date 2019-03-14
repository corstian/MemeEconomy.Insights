using GraphQL.Types;
using MemeEconomy.Insights.Graph.Types;
using Microsoft.Extensions.Configuration;
using System;

namespace MemeEconomy.Insights.Graph
{
    public class Query : ObjectGraphType<object>
    {
        public Query(IConfiguration config)
        {
            Connection<OpportunityType>()
                .Name("opportunities")
                .Argument<OpportunityOrderType>("order", "")
                .Unidirectional()
                .Resolve(context =>
                {
                    var order = context.GetArgument<string>("order");

                    
                });
        }
    }
}
