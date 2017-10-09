using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using WindowsAzure.Table;
using WindowsAzure.Table.Attributes;

namespace DmmGameRankingObserver.Core
{
    public class DmmGameRankingTableClient
    {
        private readonly CloudStorageAccount storageAccount;
        private readonly string tableName;

        readonly TableSet<DmmGameRankingEntity> table;

        public DmmGameRankingTableClient(CloudStorageAccount storageAccount, string tableName)
        {
            this.storageAccount = storageAccount;
            this.tableName = tableName;

            var tableClient = storageAccount.CreateCloudTableClient();
            table = new TableSet<DmmGameRankingEntity>(tableClient, tableName);
            table.CreateIfNotExists();
        }

        public async Task AddOrUpdateAsync(DmmGameRankingEntity entity)
        {
            await table.AddOrUpdateAsync(entity);
        }
        public async Task AddOrUpdateAsync(IEnumerable<DmmGameRankingEntity> entities)
        {
            await table.AddOrUpdateAsync(entities);
        }

        public DmmGameRankingEntity[] GetByTitle(string title)
        {
            return table.Where(x => x.GameTitle == title).ToArray();
        }
    }

    public class DmmGameRankingEntity
    {
        [PartitionKey]
        public string GameTitle { get; set; }

        [RowKey]
        public string ObservedDateTime { get; set; }

        public int Rank { get; set; }
        public string Comment { get; set; }
        public string Genre { get; set; }

        public DmmGameRankingEntity() { }
    }
}
