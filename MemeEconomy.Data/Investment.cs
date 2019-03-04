using System;

namespace MemeEconomy.Data
{
    public class Investment
    {
        public Guid Id { get; set; }
        public Guid OpportunityId { get; set; }

        public DateTime Timestamp { get; set; }
        public long Amount { get; set; }
        public int Upvotes { get; set; }

        public Opportunity Opportunity { get; set; }
    }
}
