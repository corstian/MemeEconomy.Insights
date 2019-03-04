using GraphQL.Types;
using MemeEconomy.Data;
using Microsoft.Extensions.Configuration;
using System;

namespace MemeEconomy.Insights.Graph.Types
{
    public class InvestmentType : ObjectGraphType<Investment>
    {
        public InvestmentType(
            IConfiguration config,
            MemeEconomyContext store)
        {
            Field<StringGraphType>()
                .Name("cursor")
                .Resolve(context => context.Source.Id.ToCursor());

            Field(q => q.Amount);
            Field(q => q.Timestamp);

            Field<OpportunityType>()
                .Name("opporunity")
                .ResolveAsync(async context =>
                {
                    using (var store = new MemeEconomyContext(config))
                    {
                        return await store.Opportunities.FindAsync(context.Source.OpportunityId);
                    }
                });
        }
    }
}
