using MemeEconomy.Insights.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MemeEconomy.Insights
{
    public class Ledger
    {
        // On startup, retrieve all relevant data from the db.
        // ToDo: Add an timer to remove all old posts
        private ConcurrentBag<Opportunity> _ledger = new ConcurrentBag<Opportunity>();

        public void AddOpportunity(Opportunity opportunity)
        {
            _ledger.Add(opportunity);
        }

        public void AddTransaction(Investment investment)
        {
            if (!_ledger.TryPeek(out var opportunity)) return;

            if (opportunity.Investments == null) opportunity.Investments = new List<Investment>();

            opportunity.Investments.Add(investment);
        }

        public dynamic GetStats(string post)
        {
            var investments = _ledger
                .SingleOrDefault(q => q.PostId == post)
                .Investments;

            return new
            {
                ActiveInvestments = investments
                    .Where(q => q.Timestamp > DateTime.UtcNow.AddHours(-4))
                    .Sum(q => q.Amount),
                TotalInvested = investments.Sum(q => q.Amount),
                RecentUpvotes = investments
                    .Where(q => q.Timestamp > DateTime.UtcNow.AddHours(-4))
                    .Sum(q => q.Amount),
                TotalUpvotes = investments.Sum(q => q.Upvotes)
            };
        }

        public IEnumerable<Opportunity> NewPosts => _ledger
            .Where(q => q.Timestamp > DateTime.UtcNow.AddHours(-4))
            .OrderByDescending(q => q.Timestamp);

        public IEnumerable<Opportunity> HotPosts => _ledger
            .OrderByDescending(q => q.Investments
                .Where(w => w.Timestamp > DateTime.UtcNow.AddHours(-4))
                .Sum(w => w.Amount));

        public IEnumerable<Opportunity> PopularPosts => _ledger
            .OrderByDescending(q => q.Investments.Sum(w => w.Upvotes));

        public IEnumerable<Opportunity> RisingPosts => _ledger
            .OrderByDescending(q => q.Investments
                .Where(w => w.Timestamp > DateTime.UtcNow.AddHours(-4))
                .Sum(w => w.Upvotes));
    }
}
