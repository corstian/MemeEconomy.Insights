using Boerman.GraphQL.Contrib;
using SqlKata;
using SqlKata.Execution;
using System;

namespace MemeEconomy.Insights.Queries
{
    public static partial class Extensions
    {
        public static Query OpportunityQuery(
            this QueryFactory queryFactory,
            OpportunityOrder order = default)
        {
            var sqlQuery = queryFactory.Query("Opportunities");

            sqlQuery.LeftJoinAs("Investments", "AI", "AI.OpportunityId", "Opportunities.Id");
            sqlQuery.LeftJoinAs("Investments", "RI", "RI.OpportunityId", "Opportunities.Id");

            sqlQuery.Where("Opportunities.Timestamp", ">", DateTime.UtcNow.AddHours(-24));
            sqlQuery.Where("RI.Timestamp", ">", DateTime.UtcNow.AddHours(-4));

            if (order == default) order = OpportunityOrder.New;

            switch (order)
            {
                case OpportunityOrder.New:
                    sqlQuery.OrderByDesc("Timestamp");
                    break;
                case OpportunityOrder.Hot:
                    sqlQuery.OrderByDesc("ActiveInvestments");
                    break;
                case OpportunityOrder.Popular:
                    sqlQuery.OrderByDesc("Upvotes");
                    break;
            }

            var columns = new[] {
                "Opportunities.Id",
                "Opportunities.MemeUri",
                "Opportunities.PostId",
                "Opportunities.Timestamp",
                "Opportunities.Title"
            };

            sqlQuery.Select(columns);

            sqlQuery.SelectRaw("SUM(AI.Amount) AS Investments");
            sqlQuery.SelectRaw("MAX(AI.Upvotes) AS Upvotes");
            sqlQuery.SelectRaw("SUM(RI.Amount) AS ActiveInvestments");

            sqlQuery.GroupBy(columns);

            return sqlQuery;
        }
    }
}
