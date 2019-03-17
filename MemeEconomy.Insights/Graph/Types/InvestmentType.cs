using Boerman.GraphQL.Contrib.DataLoaders;
using GraphQL.DataLoader;
using GraphQL.Types;
using MemeEconomy.Insights.Models;
using Microsoft.Extensions.Configuration;

namespace MemeEconomy.Insights.Graph.Types
{
    public class InvestmentType : ObjectGraphType<Investment>
    {
        public InvestmentType(
            IConfiguration config,
            IDataLoaderContextAccessor dataLoader,
            Ledger ledger)
        {
            Field<StringGraphType>()
                .Name("cursor")
                .Resolve(context => context.Source.Id.ToCursor());

            Field(q => q.Amount);
            Field(q => q.Upvotes);
            Field(q => q.Timestamp, type: typeof(DateTimeGraphType));

            Field<OpportunityType>()
                .Name("opportunity")
                .Resolve(context => ledger.GetOpportunity(context.Source.OpportunityId));
        }
    }
}
