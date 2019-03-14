using GraphQL.DataLoader;
using GraphQL.Types;
using MemeEconomy.Data;
using MemeEconomy.Insights.Graph.DataLoaders;
using MemeEconomy.Insights.Models;
using Microsoft.Extensions.Configuration;

namespace MemeEconomy.Insights.Graph.Types
{
    public class OpportunityType : ObjectGraphType<Opportunity>
    {
        public OpportunityType(
            IConfiguration config,
            IDataLoaderContextAccessor dataLoader)
        {
            Field<StringGraphType>()
                .Name("cursor")
                .Resolve(context => context.Source.Id.ToCursor());

            Field(q => q.Timestamp);
            Field(q => q.RedditUri);
            Field(q => q.MemeUri);

            Field<ListGraphType<InvestmentType>>()
                .Name("investments")
                .Resolve(context =>
                {
                    using (var store = new MemeEconomyContext(config))
                    {
                        return dataLoader.EntityCollectionLoader(store.Investments, q => q.OpportunityId, context.Source.Id);
                    }
                });
        }
    }
}
