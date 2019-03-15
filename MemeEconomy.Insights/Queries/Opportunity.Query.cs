using SqlKata;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemeEconomy.Insights.Queries
{
    public static partial class Extensions
    {
        public static Query OpportunityQuery(
            this QueryFactory queryFactory,
            OpportunityOrder order = default)
        {
            var sqlQuery = queryFactory.Query("Opportunities");

            if (order != default)
            {
                
            } else
            {
                sqlQuery.OrderByDesc("Timestamp");
            }

            sqlQuery.Select("*");

            return sqlQuery;
        }
    }
}
