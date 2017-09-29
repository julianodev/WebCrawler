using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace webCrawler
{
    public sealed class Crawler : Robot
    {
        public Crawler()
        {
            RobotWebClient = new RobotWebClient();
        }

        public List<Article> LoadPosts()
        {
            var document = new HtmlDocument();

            RobotWebClient.AllowAutoRedirect = false;

            var urlBase = new Uri("http://netcoders.com.br");

            var website = HttpGet(urlBase);

            var paginator = website.DocumentNode.SelectNodes("//div[@id='post-navigator']//div[@class='wp-pagenavi iegradient']//a[@href]");


            var firstPage = Convert.ToInt32(ExtractNumberPage(paginator.FirstOrDefault()?.Attributes["href"].Value));

            var lastPage = Convert.ToInt32(ExtractNumberPage(paginator.LastOrDefault()?.Attributes["href"].Value));

            var pages = new List<string>();

            pages.Add($"{urlBase}");

            for (var i = firstPage; i <= lastPage; i++)
                pages.Add(@"http://netcoders.com.br/page/" + i + "/");


            var articles = new List<Article>();

            foreach (var page in pages)
            {
                var request = HttpGet(new Uri(page));

                var articlesOrderBy =
                    request.
                    DocumentNode.
                    Descendants()
                    .Where(n => n.Name == "article")
                    .OrderByDescending(orderby => orderby.Id).ToList();

                foreach (var item in articlesOrderBy)
                {
                    var article = new Article();

                    document.LoadHtml(item.InnerHtml);

                    article.Image = HtmlEntity.DeEntitize(ConvertUtf(document.DocumentNode.SelectSingleNode("//img")
                        .Attributes["src"].Value));

                    article.Title = HtmlEntity.DeEntitize(ConvertUtf(document.DocumentNode.DescendantsAndSelf()
                        .FirstOrDefault(d => d.Attributes["class"] != null && d.Attributes["class"].Value == "post-title entry-title")
                        ?.InnerText));

                    article.Date = HtmlEntity.DeEntitize(ConvertUtf(document.DocumentNode.DescendantsAndSelf()
                        .FirstOrDefault(d => d.Name == "span" && d.Attributes["class"].Value == "post-time")
                        ?.InnerText));

                    article.Description = HtmlEntity.DeEntitize(ConvertUtf(document.DocumentNode.DescendantsAndSelf()
                        .FirstOrDefault(d => d.Attributes["class"] != null && d.Attributes["class"].Value == "entry-content")
                        ?.InnerText));

                    article.Author = HtmlEntity.DeEntitize(ConvertUtf(document.DocumentNode.DescendantsAndSelf()
                        .FirstOrDefault(d => d.Attributes["class"] != null && d.Attributes["class"].Value == "post-author")
                        ?.InnerText));


                    article.Link = HtmlEntity.DeEntitize(ConvertUtf(document.DocumentNode.SelectSingleNode("//a")
                        .Attributes["href"].Value));


                    articles.Add(article);
                }
            }



            return articles.ToList();
        }


        private static string ConvertUtf(string text)
        {
            var data = Encoding.Default.GetBytes(text);
            var textConverted = Encoding.UTF8.GetString(data);
            return textConverted;
        }

        private string ExtractNumberPage(string page)
        {
            var regex = new Regex(@"\d+");
            return regex.Match(page).Value;
        }

        private static byte[] GetBytes(string text)
        {
            return Encoding.Default.GetBytes(text);
        }
    }
}
