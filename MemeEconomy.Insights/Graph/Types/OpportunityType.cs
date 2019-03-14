using Boerman.GraphQL.Contrib.DataLoaders;
using GraphQL.DataLoader;
using GraphQL.Types;
using MemeEconomy.Insights.Models;
using Microsoft.Extensions.Configuration;

namespace MemeEconomy.Insights.Graph.Types
{
    public class OpportunityType : ObjectGraphType<Opportunity>
    {
        public OpportunityType(
            IDataLoaderContextAccessor dataLoader,
            IContextProvider<MemeEconomyContext> dbProvider)
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
                    return dataLoader.EntityCollectionLoader(dbProvider.Get().Investments, q => q.OpportunityId, context.Source.Id);
                });
        }
    }
}
