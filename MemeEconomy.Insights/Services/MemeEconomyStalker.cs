using Microsoft.Extensions.Hosting;
using RedditSharp;
using RedditSharp.Things;
using System.Threading;
using System.Threading.Tasks;

namespace MemeEconomy.Insights.Services
{
    public class MemeEconomyStalker : IHostedService
    {
        private readonly Reddit _reddit;
        private RedditUser _memeInvestorBot;

        public MemeEconomyStalker(Reddit reddit)
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
                    /*
                     * Because investments are acknowledged by the MemeInvestor_bot we should be fine
                     * by just parsing all comments written by this bot. If we catch the updates we should
                     * also be able to see the return on investment etc.
                     */

                    
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
