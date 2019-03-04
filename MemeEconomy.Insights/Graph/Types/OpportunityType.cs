using GraphQL.Types;
using MemeEconomy.Data;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace MemeEconomy.Insights.Graph.Types
{
    public class OpportunityType : ObjectGraphType<Opportunity>
    {
        public OpportunityType(IConfiguration config)
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
                        return store.Investments.Where(q => q.OpportunityId == context.Source.Id);
                    }
                });
        }
    }
}
