using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DmmGameRankingObserver
{
    public static class ObserveRankingPageTimerTrigger
    {
        [FunctionName("ObserveRankingPageTimerTrigger")]
        public static async Task Run([TimerTrigger("0 0 */1 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            //var client = new ArcanaHeart3lmsssRankingClient();

            //var blobContainer = await GetBlobContainerAsync();
        }
    }
}
