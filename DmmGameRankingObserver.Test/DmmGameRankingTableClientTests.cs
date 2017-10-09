using Microsoft.VisualStudio.TestTools.UnitTesting;
using DmmGameRankingObserver.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;

namespace DmmGameRankingObserver.Core.Tests
{
    [TestClass()]
    public class DmmGameRankingTableClientTests
    {
        static readonly string TableName = "TestDmmGameRanking";
        CloudStorageAccount StorageAccount => CloudStorageAccount.DevelopmentStorageAccount;

        [TestInitialize]
        public void TestInitialize()
        {
            DeleteTestTable();
        }

        private void DeleteTestTable()
        {
            var tableClient = StorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(TableName);
            table.DeleteIfExists();
        }


        [TestMethod()]
        public async Task UpdateOrInsertAsyncTest()
        {
            var client = new DmmGameRankingTableClient(StorageAccount, TableName);

            var entity = new DmmGameRankingEntity
            {
                GameTitle = "ul4",
                ObservedDateTime = DateTime.Now.ToString("yyyyMMdd_hhmmss"),
                Rank = 1,
                Comment = "コメントだよー",
                Genre = "「ジャンル」のスペル難しいよー",
            };

            await client.AddOrUpdateAsync(entity);
            client.GetByTitle("ul4").Length.Is(1);
        }

        [TestMethod()]
        public async Task UpdateOrInsertsAsyncTest()
        {
            var client = new DmmGameRankingTableClient(StorageAccount, TableName);

            var nowStr = DateTime.Now.ToString("yyyyMMdd_hhmmss");
            var entities = new DmmGameRankingEntity[] 
            {
                new DmmGameRankingEntity
                {
                    GameTitle = "ul4",
                    ObservedDateTime = nowStr,
                    Rank = 1,
                    Comment = "コメントだよー",
                    Genre = "「ジャンル」のスペル難しいよー",
                },
                new DmmGameRankingEntity
                {
                    GameTitle = "sf5ae",
                    ObservedDateTime = nowStr,
                    Rank = 2,
                    Comment = "コメントだよー2",
                    Genre = "「ジャンル」のスペル難しいよー2",
                }
            };

            await client.AddOrUpdateAsync(entities);
            client.GetByTitle("ul4").Length.Is(1);
            client.GetByTitle("sf5ae").Length.Is(1);
        }
    }
}