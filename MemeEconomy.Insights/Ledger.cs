using MemeEconomy.Insights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace MemeEconomy.Insights
{
    public class Ledger
    {
        private List<Opportunity> _ledger = new List<Opportunity>();
        private Timer _timer = new Timer(60000);

        public Ledger()
        {
            _timer.Elapsed += ClearOldOpportunities;
            _timer.Start();
        }

        private void ClearOldOpportunities(object sender, ElapsedEventArgs e)
        {
            _ledger.RemoveAll(q => q.Timestamp < DateTime.UtcNow.AddDays(-1));
        }

        public void AddOpportunity(Opportunity opportunity)
        {
            _ledger.Add(opportunity);
        }

        public void AddTransaction(Investment investment)
        {
            var opportunity = _ledger.SingleOrDefault(q => q.Id == investment.OpportunityId);

            if (opportunity?.Investments == null) opportunity.Investments = new List<Investment>();

            opportunity.Investments.Add(investment);
        }

        public IEnumerable<Investment> GetInvestments(Guid opportunityId) => _ledger
            .SingleOrDefault(q => q.Id == opportunityId)
            ?.Investments
            ?.GroupBy(q => new
            {
                q.Timestamp,
                q.Amount
            })
            .Select(q => q.First())
            ?? new Investment[] { };

        public object GetOpportunity(Guid opportunityId) => _ledger.SingleOrDefault(q => q.Id == opportunityId);

        public IEnumerable<Opportunity> NewPosts => _ledger
            .Where(q => q.Timestamp > DateTime.UtcNow.AddHours(-4))
            .OrderByDescending(q => q.Timestamp);

        public IEnumerable<Opportunity> HotPosts => _ledger
            .OrderByDescending(q => q.Investments?
                .Where(w => w.Timestamp > DateTime.UtcNow.AddHours(-4))
                .Sum(w => w.Amount) ?? 0);

        public IEnumerable<Opportunity> PopularPosts => _ledger
            .OrderByDescending(q => q.Investments?.Sum(w => w.Upvotes) ?? 0);

        public IEnumerable<Opportunity> RisingPosts => _ledger
            .OrderByDescending(q => q.Investments?
                .Where(w => w.Timestamp > DateTime.UtcNow.AddHours(-4))
                .Sum(w => w.Upvotes) ?? 0);
    }
}
