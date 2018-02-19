using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using static LiteParcer.Logik.Assets;

namespace LiteParcer.Logik
{
    class Html_Analyzer
    {
        private string tag { get; set; }
        public Html_Analyzer(string tag)
        {
            this.tag = tag;
        }

        public async Task<UrlCount> TagSearchAsync(string URLs)
        {
            return await Task.Run(()=> 
            {
                int count;
                count = GetTagCount(GetHtml(ref URLs));
                UrlCount URLCount = new UrlCount { Url = URLs, Count = count };
                return URLCount;
            });

        }

        private dynamic GetHtml(ref string url)
        {
            try
            {
                WebRequest httpWebRequest = WebRequest.Create(url);
                httpWebRequest.Method = "GET";
                string responseFromServer = string.Empty;
                try
                {
                    using (WebResponse response = httpWebRequest.GetResponse())
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(dataStream))
                            {
                                responseFromServer = reader.ReadToEnd();
                            }
                        }
                    }
                    return responseFromServer;
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
            catch (System.UriFormatException ex)
            {
                return ex;
            }
        }
        private int GetTagCount(dynamic content)
        {
            try
            {
                var html = content as string;
                HtmlDocument HtmlDocument = new HtmlDocument();
                HtmlDocument.LoadHtml(html);
                int count = 0;
                foreach (HtmlNode link in HtmlDocument.DocumentNode.SelectNodes($@"//{tag}"))
                {
                    count++;
                }
                return count;
            }
            catch (System.ArgumentNullException)
            {
                var exception = content as Exception;
                ExceptionCatch.WriteException(exception);
                return -1;
            }
            catch (Exception ex)
            {
                ExceptionCatch.WriteException(ex);
                return-1;
            }
        }

        
    }
}
