using GraphQL.Types;
using MemeEconomy.Insights.Models;
using System;
using System.Linq;

namespace MemeEconomy.Insights.Graph.Types
{
    public class OpportunityType : ObjectGraphType<Opportunity>
    {
        public OpportunityType(Ledger ledger)
        {
            Field<StringGraphType>()
                .Name("cursor")
                .Resolve(context => context.Source.Id.ToCursor());

            Field(q => q.Timestamp, type: typeof(DateTimeGraphType))
                .Description("When this opportunity has been brought to the market");

            Field(q => q.RedditUri);
            Field(q => q.MemeUri);
            Field(q => q.Title);

            Field<ListGraphType<InvestmentType>>()
                .Name("investments")
                .Resolve(context =>
                {
                    return ledger.GetInvestments(context.Source.Id);
                });

            Field<ULongGraphType>()
                .Name("nuv")
                .Description("Net updoot value")
                .Resolve(context =>
                {
                    var investments = ledger.GetInvestments(context.Source.Id) ?? new Investment[] { };

                    if (!investments.Any()) return 0;

                    var max = investments.Max(q => q.Upvotes);

                    if (max == 0) return 0;

                    return investments.Sum(q => q.Amount) / max;
                });

            Field<ULongGraphType>()
                .Name("rnuv")
                .Description("Recent net updoot value")
                .Resolve(context =>
                {
                    var investments = ledger.GetInvestments(context.Source.Id)
                        ?.Where(q => q.Timestamp > DateTime.UtcNow.AddHours(-4));

                    if (!(investments?.Any() ?? false)) return 0;

                    var max = investments.Max(q => q.Upvotes);

                    if (max == 0) return 0;

                    return investments.Sum(q => q.Amount) / max;
                });

            Field<ULongGraphType>()
                .Name("invested")
                .Resolve(context =>
                {
                    var investments = ledger.GetInvestments(context.Source.Id);

                    return investments?.Any() ?? false
                        ? investments.Sum(q => q.Amount)
                        : 0;
                });

            Field<IntGraphType>()
                .Name("upvoted")
                .Resolve(context => {
                    var investments = ledger.GetInvestments(context.Source.Id);

                    return investments?.Any() ?? false
                        ? investments.Max(q => q.Upvotes)
                        : 0;
                });

            Field<ULongGraphType>()
                .Name("recentInvestments")
                .Resolve(context =>
                {
                    var investments = ledger.GetInvestments(context.Source.Id)
                        ?.Where(q => q.Timestamp > DateTime.UtcNow.AddHours(-4));

                    if (!(investments?.Any() ?? false)) return 0;

                    return investments.Sum(q => q.Amount);
                });

            Field<IntGraphType>()
                .Name("recentUpvotes")
                .Resolve(context =>
                {
                    var investments = ledger.GetInvestments(context.Source.Id)
                        ?.Where(q => q.Timestamp > DateTime.UtcNow.AddHours(-4));

                    if (!(investments?.Any() ?? false)) return 0;

                    return investments.Max(q => q.Upvotes);
                });
        }
    }
}
