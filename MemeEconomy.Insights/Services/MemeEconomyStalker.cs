using MemeEconomy.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RedditSharp;
using RedditSharp.Things;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MemeEconomy.Insights.Services
{
    public class MemeEconomyStalker : IHostedService
    {
        private readonly Reddit _reddit;
        private RedditUser _memeInvestorBot;

        private readonly Regex _checkInvestment = new Regex(@"\*([0-9,]+) MemeCoins invested @ ([0-9,]+) upvotes\*");
        
        public MemeEconomyStalker(
            IConfiguration config,
            Reddit reddit)
        {
            _reddit = reddit;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _memeInvestorBot = _reddit.GetUser("MemeInvestor_bot");

            _ = Task.Run(() =>
            {
                foreach (var comment in _memeInvestorBot.Comments.GetListingStream())
                {
                    try
                    {
                        var match = _checkInvestment.Match(comment.Body);

                        if (match.Success)
                        {
                            var investment = new Investment
                            {
                                Amount = Convert.ToInt64(match.Groups[1].Value.Replace(",", "")),
                                Upvotes = Convert.ToInt32(match.Groups[2].Value.Replace(",", "")),
                                Timestamp = comment.Created.UtcDateTime
                            };
                        }
                        else if (comment.Body.Contains("**INVESTMENTS GO HERE - ONLY DIRECT REPLIES TO ME WILL BE PROCESSED**"))
                        {
                            // We have a new post. Retrieve information.
                            var opportunity = new Opportunity
                            {
                                Title = comment.LinkTitle,
                                Timestamp = comment.Created.UtcDateTime,
                                PostId = comment.LinkId.Split('_')[1],
                                MemeUri = _reddit.GetPost(new Uri(comment.Shortlink)).Url.ToString()
                            };
                        }

                        // We're not handling anything else rn
                    } catch (Exception)
                    {
                        // ToDo: Add error handling
                    }
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
