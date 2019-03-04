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
                .Argument<StringGraphType>("opportunity", "Accepts an opportunity's cursor in order to subscribe to them updates.")
                .Resolve(context => context.Source as Investment)
                .Subscribe(context =>
                {
                    context.GetArgument<string>("opportunity");
                    throw new NotImplementedException();
                });
        }
    }
}
