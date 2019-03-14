using Boerman.GraphQL.Contrib;
using GraphQL.Types;
using MemeEconomy.Insights.Graph.Types;
using MemeEconomy.Insights.Models;
using MemeEconomy.Insights.Queries;
using Microsoft.Extensions.Configuration;
using SqlKata.Execution;

namespace MemeEconomy.Insights.Graph
{
    public class Query : ObjectGraphType<object>
    {
        public Query(
            IConfiguration config,
            QueryFactory queryFactory)
        {
            Connection<OpportunityType>()
                .Name("opportunities")
                .Argument<OpportunityOrderType>("order", "")
                .Bidirectional()
                .ResolveAsync(async context =>
                {
                    var order = context.GetArgument<OpportunityOrder>("order");

                    return await queryFactory
                        .OpportunityQuery(order)
                        .ToConnection<Opportunity, object>(context);
                });
        }
    }
}
