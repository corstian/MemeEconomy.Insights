﻿using MemeEconomy.Insights.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RedditSharp;
using RedditSharp.Things;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MemeEconomy.Insights.Services
{
    public class MemeEconomyStalker : IHostedService
    {
        private readonly IConfiguration _config;
        private readonly Reddit _reddit;
        private readonly Ledger _ledger;
        private RedditUser _memeInvestorBot;

        private readonly Regex _checkInvestment = new Regex(@"\*([0-9,]+) MemeCoins invested @ ([0-9,]+) upd00ts\*");
        
        public MemeEconomyStalker(
            IConfiguration config,
            Reddit reddit,
            Ledger ledger)
        {
            _config = config;
            _reddit = reddit;
            _ledger = ledger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Populate the ledger with existing data

            using (var dbContext = new MemeEconomyContext(_config))
            {
                dbContext.Opportunities
                    .Include(q => q.Investments)
                    .Where(q => q.Timestamp > DateTime.UtcNow.AddDays(-1))
                    .ToList()
                    .ForEach(q => _ledger.AddOpportunity(q));
            }

            _memeInvestorBot = _reddit.GetUser("MemeInvestor_bot");

            _ = Task.Run(() =>
            {
                foreach (var comment in _memeInvestorBot.Comments.GetListingStream())
                {
                    try
                    {
                        var match = _checkInvestment.Match(comment.Body);
                        var postId = comment.LinkId.Split('_')[1];
                        //Guid opportunityId = Guid.Empty;

                        using (var store = new MemeEconomyContext(_config))
                        {
                            if (comment.Body.Contains("**INVESTMENTS GO HERE - ONLY DIRECT REPLIES TO ME WILL BE PROCESSED**"))
                            {
                                var opportunity = new Opportunity
                                {
                                    Id = Guid.NewGuid(),
                                    Title = comment.LinkTitle,
                                    Timestamp = comment.Created.UtcDateTime,
                                    PostId = postId,
                                    MemeUri = _reddit.GetPost(new Uri(comment.Shortlink)).Url.ToString()
                                };

                                _ledger.AddOpportunity(opportunity);

                                Program.Opportunities.OnNext(opportunity);

                                store.Opportunities.Add(opportunity);
                                store.SaveChanges();
                            }
                            else if (match.Success)
                            {
                                var opportunityId = store
                                    .Opportunities
                                    ?.SingleOrDefault(q => q.PostId == postId)
                                    ?.Id ?? Guid.Empty;
                            
                                if (opportunityId == Guid.Empty)
                                {
                                    var opportunity = new Opportunity
                                    {
                                        Id = Guid.NewGuid(),
                                        Title = comment.LinkTitle,
                                        Timestamp = comment.Created.UtcDateTime,
                                        PostId = postId,
                                        MemeUri = _reddit.GetPost(new Uri(comment.Shortlink)).Url.ToString()
                                    };

                                    _ledger.AddOpportunity(opportunity);

                                    store.Opportunities.Add(opportunity);
                                    store.SaveChanges();

                                    opportunityId = opportunity.Id;
                                }

                                var investment = new Investment
                                {
                                    Id = Guid.NewGuid(),
                                    OpportunityId = opportunityId,
                                    Timestamp = comment.Created.UtcDateTime,
                                    Amount = Convert.ToInt64(match.Groups[1].Value.Replace(",", "")),
                                    Upvotes = Convert.ToInt32(match.Groups[2].Value.Replace(",", ""))
                                };

                                _ledger.AddTransaction(investment);

                                Program.Investments.OnNext(investment);

                                store.Investments.Add(investment);
                                store.SaveChanges();
                            }
                        }
                    } catch (Exception)
                    {
                        
                    }
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // ToDo: Remove all duplicate records from the db

            return Task.CompletedTask;
        }
    }
}
