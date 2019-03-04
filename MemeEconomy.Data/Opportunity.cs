using System;
using System.Collections.Generic;

namespace MemeEconomy.Data
{
    public class Opportunity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public DateTime Timestamp { get; set; }
        public string RedditUri { get; set; }
        public string MemeUri { get; set; }

        public List<Investment> Investments { get; set; }
    }
}
