using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;

namespace DmmGameRankingObserver.Core
{
    public class DmmGameRankingClient
    {
        static readonly string RankingPageUrl = @"http://games.dmm.co.jp/ranking/";
        HttpClient httpClient = new HttpClient();

        IHtmlDocument document;

        public async Task LoadFromWebAsync()
        {
            var html = await httpClient.GetStringAsync(RankingPageUrl);
            await LoadFromHtmlAsync(html);
        }
        public async Task LoadFromHtmlAsync(string html)
        {
            var parser = new HtmlParser();
            document = await parser.ParseAsync(html);
        }

        public async Task DownloadRankingPageHtml(string output)
        {
            var html = await httpClient.GetStringAsync(RankingPageUrl);
            File.WriteAllText(output, html);
        }

        public IList<GameRanking> Parse()
        {
            var listNode = document.QuerySelectorAll("div")
                            .Where(x => x.ClassList.Contains("d-item"))
                            .Where(x => x.ClassList.Contains("ntg-item-pclist"))
                            .Where(x => x.ClassList.Contains("ranking"))
                            .Single();

            var ul = listNode.QuerySelectorAll("ul").First();
            var liElements = ul.Children.Where(x => x.NodeName.ToLower() == "li").ToArray();

            var results = new List<GameRanking>();
            foreach (var liNode in liElements)
            {
                var name = liNode.QuerySelectorAll("span")
                                .Where(x => x.ClassList.Contains("thumb"))
                                .Children("img")
                                .First()
                                .GetAttribute("alt");
                var rank = liNode.QuerySelectorAll("span")
                                .Where(x => x.ClassList.Contains("tx-rank"))
                                .First()
                                .TextContent
                                .ToInt();
                var comment = liNode.QuerySelectorAll("span")
                                .Where(x => x.ClassList.Contains("tx-comment"))
                                .First()
                                .TextContent;
                var genre = liNode.QuerySelectorAll("a")
                                .Where(x => x.ClassList.Contains("ntg-tx-genre"))
                                .First()
                                .TextContent;

                var elem = new GameRanking
                {
                    Name = name,
                    Rank = rank,
                    Comment = comment,
                    Genre = genre,
                };
                results.Add(elem);
            }

            return results;
        }
    }

    static class StringEx
    {
        public static int ToInt(this string str) => int.Parse(str);
    }
}
