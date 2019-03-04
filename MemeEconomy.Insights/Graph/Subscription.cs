using GraphQL.Types;
using MemeEconomy.Data;
using MemeEconomy.Insights.Graph.Types;
using System;

namespace MemeEconomy.Insights.Graph
{
    public class Subscription : ObjectGraphType<object>
    {
        public Subscription()
        {
            Field<OpportunityType>()
                .Name("opportunities")
                .Resolve(context => context.Source as Opportunity)
                .Subscribe(context =>
                {
                    throw new NotImplementedException();
                });

            Field<InvestmentType>()
                .Name("investments")
                .Resolve(context => context.Source as Investment)
                .Subscribe(context =>
                {
                    throw new NotImplementedException();
                });
        }
    }
}
