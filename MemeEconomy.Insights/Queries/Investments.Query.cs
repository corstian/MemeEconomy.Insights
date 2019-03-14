using SqlKata;
using SqlKata.Execution;
using System;
using System.Collections.Generic;

namespace MemeEconomy.Insights.Queries
{
    public static partial class Extensions
    {
        public static Query InvestmentsQuery(
            this QueryFactory queryFactory)
        {
            var sqlQuery = queryFactory.Query("Investments");

            

            return sqlQuery;
        }
    }
}
