using GraphQL.Types;
using MemeEconomy.Data;
using System;

namespace MemeEconomy.Insights.Graph.Types
{
    public class OpportunityType : ObjectGraphType<Opportunity>
    {
        public OpportunityType()
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
                    throw new NotImplementedException();
                });
        }
    }
}
