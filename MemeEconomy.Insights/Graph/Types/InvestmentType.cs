using GraphQL.DataLoader;
using GraphQL.Types;
using MemeEconomy.Data;
using MemeEconomy.Insights.Graph.DataLoaders;
using MemeEconomy.Insights.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace MemeEconomy.Insights.Graph.Types
{
    public class InvestmentType : ObjectGraphType<Investment>
    {
        public InvestmentType(
            IConfiguration config,
            IDataLoaderContextAccessor dataLoader)
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
                        return await dataLoader.EntityLoader(store.Opportunities, q => q.Id, context.Source.OpportunityId);
                    }
                });
        }
    }
}
