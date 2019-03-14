using GraphQL.Types;
using MemeEconomy.Data;
using MemeEconomy.Insights.Graph.Types;
using MemeEconomy.Insights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

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
                    return Program.Opportunities.AsObservable();
                });

            Field<InvestmentType>()
                .Name("investments")
                .Argument<ListGraphType<StringGraphType>>("opportunities", "Accepts an opportunity's cursor in order to subscribe to them updates.")
                .Resolve(context => context.Source as Investment)
                .Subscribe(context =>
                {
                    var opportunities = context
                        .GetArgument<List<string>>("opportunities")
                            ?.Select(q => q.FromCursor())
                        ?? new List<Guid>(0);

                    var flow = Program.Investments.AsObservable();

                    if (opportunities.Any()) flow.Where(q => opportunities.Contains(q.OpportunityId));

                    return flow;
                });
        }
    }
}
