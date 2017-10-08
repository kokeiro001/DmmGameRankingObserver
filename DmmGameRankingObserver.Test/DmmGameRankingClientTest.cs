using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DmmGameRankingObserver.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DmmGameRankingObserver.Test
{
    [TestClass]
    public class DmmGameRankingClientTest
    {
        static readonly string LocalHtmlPath = @"C:/tmp/DmmGameRankingObserver/dmmrankigpage.html";

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            var dir = Path.GetDirectoryName(LocalHtmlPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        [TestMethod]
        public async Task DownloadHtml()
        {
            // テストでも何でもない、うんちみたいなコード。
            // ローカルにHTMLファイルをダウンロードしてくるテスト。

            if (!File.Exists(LocalHtmlPath))
            {
                var client = new DmmGameRankingClient();
                await client.DownloadRankingPageHtml(LocalHtmlPath);
            }
            File.Exists(LocalHtmlPath).IsTrue();
        }

        [TestMethod]
        public async Task ParseTest()
        {
            var client = new DmmGameRankingClient();
            var html = File.ReadAllText(LocalHtmlPath);
            await client.LoadFromHtmlAsync(html);
            var elements = client.Parse();
            elements.Any().IsTrue();
        }
    }
}
