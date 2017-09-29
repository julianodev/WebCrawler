using System;
using System.Net;

namespace webCrawler
{
    public class RobotWebClient : WebClient
    {
        public CookieContainer Cookie = new CookieContainer();
        public bool AllowAutoRedirect { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (!(request is HttpWebRequest)) return request;

            (request as HttpWebRequest).ServicePoint.Expect100Continue = false;
            (request as HttpWebRequest).CookieContainer = Cookie;
            (request as HttpWebRequest).KeepAlive = false;
            (request as HttpWebRequest).AllowAutoRedirect = AllowAutoRedirect;

            try
            {
               
                return request;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
         
        }
    }
}
