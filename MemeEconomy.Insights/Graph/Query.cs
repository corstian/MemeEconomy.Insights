using GraphQL.Types;
using MemeEconomy.Insights.Graph.Types;
using System;

namespace MemeEconomy.Insights.Graph
{
    public class Query : ObjectGraphType<object>
    {
        public Query()
        {
            Field<OpportunityType>()
                .Name("opportunities")
                .Resolve(context =>
                {
                    throw new NotImplementedException();
                });

            Field<InvestmentType>()
                .Name("investments")
                .Resolve(context =>
                {
                    throw new NotImplementedException();
                });
        }
    }
}
