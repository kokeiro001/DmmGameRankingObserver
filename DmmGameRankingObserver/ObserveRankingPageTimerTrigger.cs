using System;
using System.Linq;
using System.Threading.Tasks;
using DmmGameRankingObserver.Core;
using Microsoft.Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;

namespace DmmGameRankingObserver
{
    public static class ObserveRankingPageTimerTrigger
    {
        static CloudStorageAccount StorageAccount =>
            CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("AzureWebJobsStorage"));

        static readonly string TableName = @"DmmGameRanking";

        // 毎時0分に更新されてる可能性があるため、毎時10分に取得するように。
        [FunctionName("ObserveRankingPageTimerTrigger")]
        public static async Task Run([TimerTrigger("0 10 */1 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            var nowStr = DateTime.Now.ToString("yyyyMMdd_hhmmss");
            log.Info($"nowStr: {nowStr}");

            var dmmClient = new DmmGameRankingClient();

            log.Info($"dmmClient.LoadFromWebAsync()");
            await dmmClient.LoadFromWebAsync();

            log.Info($"dmmClient.Parse()");
            var rakingData = dmmClient.Parse();

            log.Info($"new tableClient");
            var tableClient = new DmmGameRankingTableClient(StorageAccount, TableName);

            log.Info($"mapping rankingData to entity");
            var entities = rakingData.Select(x => new DmmGameRankingEntity
            {
                GameTitle = x.Name,
                ObservedDateTime = nowStr,
                Rank = x.Rank,
                Genre = x.Genre,
                Comment = x.Comment,
            });

            log.Info($"tableClient.AddOrUpdateAsync(entities)");
            await tableClient.AddOrUpdateAsync(entities);

            log.Info($"finished!!");
        }

    }
}
