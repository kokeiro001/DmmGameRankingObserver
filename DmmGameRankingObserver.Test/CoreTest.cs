using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DmmGameRankingObserver.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;

namespace DmmGameRankingObserver.Test
{
    [TestClass]
    public class CoreTest
    {
        static readonly string LocalHtmlPath = @"C:/tmp/DmmGameRankingObserver/dmmrankigpage.html";
        static readonly string TableName = "TestDmmGameRanking";
        CloudStorageAccount StorageAccount => CloudStorageAccount.DevelopmentStorageAccount;

        [TestMethod]
        public async Task TestMethod1()
        {
            // delete table
            StorageAccount
                .CreateCloudTableClient()
                .GetTableReference(TableName)
                .DeleteIfExists();

            // load, parse
            var client = new DmmGameRankingClient();
            var html = File.ReadAllText(LocalHtmlPath);
            await client.LoadFromHtmlAsync(html);
            var elements = client.Parse();
            elements.Any().IsTrue();

            var tableClient = new DmmGameRankingTableClient(StorageAccount, TableName);
            var nowStr = DateTime.Now.ToString("yyyyMMdd_hhmmss");
            var entities = elements.Select(x => new DmmGameRankingEntity
            {
                GameTitle = x.Name,
                ObservedDateTime = nowStr,
                Rank = x.Rank,
                Genre = x.Genre,
                Comment = x.Comment,
            });
            await tableClient.AddOrUpdateAsync(entities);
        }
    }
}
