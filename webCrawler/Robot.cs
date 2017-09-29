using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;

namespace webCrawler
{
    public class Robot
    {
        public RobotWebClient RobotWebClient { get; set; }

        public HtmlDocument HttpGet(Uri url)
        {
            lock (url)
            {
                var htmlDocument = new HtmlDocument();


                RobotWebClient.Headers[HttpRequestHeader.Host] = "netcoders.com.br";
                RobotWebClient.Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                RobotWebClient.Headers[HttpRequestHeader.AcceptEncoding] = "deflate";
                RobotWebClient.Headers[HttpRequestHeader.AcceptLanguage] = "en-US,pt-BR;q=0.8,pt;q=0.6,en;q=0.4";
                RobotWebClient.Headers[HttpRequestHeader.CacheControl] = "max-age=0";
                RobotWebClient.Headers[HttpRequestHeader.Cookie] = "";
                RobotWebClient.Headers[HttpRequestHeader.Referer] = "http://netcoders.com.br/";
                RobotWebClient.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.101 Safari/537.36";
                htmlDocument.LoadHtml(RobotWebClient.DownloadString(url));
                return htmlDocument;
            }
        }

        public HtmlDocument HttpPost(Uri url, NameValueCollection paramaters)
        {
            var htmlDocument = new HtmlDocument();
            var page = RobotWebClient.UploadValues(url, paramaters);
            htmlDocument.Load(Encoding.Default.GetString(page, 0, page.Length));
            return htmlDocument;
        }
    }
}
