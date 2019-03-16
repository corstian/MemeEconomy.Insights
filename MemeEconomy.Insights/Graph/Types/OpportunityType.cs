using Boerman.GraphQL.Contrib.DataLoaders;
using GraphQL.DataLoader;
using GraphQL.Types;
using MemeEconomy.Insights.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            Field(q => q.Timestamp, type: typeof(DateTimeGraphType))
                .Description("When this opportunity has been brought to the market");

            Field(q => q.RedditUri);
            Field(q => q.MemeUri);
            Field(q => q.Title);

            Field<ULongGraphType>()
                .Name("netAssetValue")
                .Description("Net Asset Value per updoot")
                .ResolveAsync(async context =>
                {
                    var investments = (await GetInvestments(dataLoader, dbProvider, context)) ?? new Investment[] { };

                    return investments.Any()
                        ? investments.Sum(q => q.Amount) / investments.Max(q => q.Upvotes)
                        : 0;
                });

            Field<ULongGraphType>()
                .Name("totalInvested")
                .ResolveAsync(async context => (await GetInvestments(dataLoader, dbProvider, context))?.Max(q => q.Amount) ?? 0);

            Field<IntGraphType>()
                .Name("totalUpvotes")
                .ResolveAsync(async context => (await GetInvestments(dataLoader, dbProvider, context))?.Max(q => q.Upvotes) ?? 0);

            Field<ListGraphType<InvestmentType>>()
                .Name("investments")
                .ResolveAsync(async context => ((await GetInvestments(dataLoader, dbProvider, context)) ?? new Investment[] { }).OrderBy(q => q.Timestamp));
        }
        
        private async Task<IEnumerable<Investment>> GetInvestments(
            IDataLoaderContextAccessor dataLoader,
            IContextProvider<MemeEconomyContext> dbProvider,
            ResolveFieldContext<Opportunity> context)
        {
            var investments = await dataLoader.EntityCollectionLoader(dbProvider.Get().Investments, q => q.OpportunityId, context.Source.Id);

            if (investments == null || !investments.Any()) return null;

            // Deduplication logic
            investments = investments.GroupBy(q => new
            {
                q.Timestamp,
                q.Amount
            })
            .Select(q => q.First());

            return investments;
        }
    }
}
