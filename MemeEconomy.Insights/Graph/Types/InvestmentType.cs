using GraphQL.Types;
using MemeEconomy.Data;
using System;

namespace MemeEconomy.Insights.Graph.Types
{
    public class InvestmentType : ObjectGraphType<Investment>
    {
        public InvestmentType()
        {
            Field<StringGraphType>()
                .Name("cursor")
                .Resolve(context => context.Source.Id.ToCursor());

            Field(q => q.Amount);
            Field(q => q.Timestamp);

            Field<OpportunityType>()
                .Name("opporunity")
                .Resolve(context =>
                {
                    // ToDo: Return new opportunity instance

                    throw new NotImplementedException();
                });
        }
    }
}
