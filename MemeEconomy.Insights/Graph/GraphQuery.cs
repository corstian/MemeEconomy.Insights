using GraphQL.Relay.Types;
using GraphQL.Types;
using MemeEconomy.Insights.Graph.Types;
using System;

namespace MemeEconomy.Insights.Graph
{
    public class GraphQuery : ObjectGraphType<object>
    {
        public GraphQuery(Ledger ledger)
        {
            Connection<OpportunityType>()
                .Name("opportunities")
                .Argument<OpportunityOrderType>("order", "")
                .Unidirectional()
                .PageSize(100)
                .Resolve(context =>
                {
                    var order = context.GetArgument<OpportunityOrder>("order");

                    switch (order)
                    {
                        case OpportunityOrder.Hot:
                            return ConnectionUtils.ToConnection(ledger.HotPosts, context);
                        case OpportunityOrder.New:
                            return ConnectionUtils.ToConnection(ledger.NewPosts, context);
                        case OpportunityOrder.Popular:
                            return ConnectionUtils.ToConnection(ledger.PopularPosts, context);
                        case OpportunityOrder.Rising:
                            return ConnectionUtils.ToConnection(ledger.RisingPosts, context);
                        default: throw new NotImplementedException();
                    }
                });
        }
    }
}
