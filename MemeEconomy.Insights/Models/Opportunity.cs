using Boerman.GraphQL.Contrib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemeEconomy.Insights.Models
{
    public class Opportunity : IId
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public DateTime Timestamp { get; set; }

        public string PostId { get; set; }

        public string MemeUri { get; set; }

        public List<Investment> Investments { get; set; }


        [NotMapped]
        public string RedditUri => $"https://reddit.com/r/MemeEconomy/comments/{PostId}/";
    }
}
