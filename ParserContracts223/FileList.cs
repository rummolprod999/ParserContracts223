using System;
using System.IO;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace ParserContracts223
{
    public class FileList
    {
        private static string _urlClearspending = "https://clearspending.ru/opendata/";

        public static void GetUrl()
        {
            var request = WebRequest.Create(_urlClearspending);
            using (var responses = request.GetResponse())
            {
                using (var streams = responses.GetResponseStream())
                using (var readers = new StreamReader(streams))
                {
                    string html = readers.ReadToEnd();
//                   Console.WriteLine(html);
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);
                    Console.WriteLine(doc.ToString());
                    HtmlNodeCollection c =
                        doc.DocumentNode.SelectNodes("//a[contains(@href,'/download/opendata/contracts_223fz')]");
                    Console.WriteLine(c.ToString());
                    if (c != null)
                    {
                        foreach (HtmlNode n in c)
                        {
                            if (n.Attributes["href"] != null)
                            {
                                string u = n.Attributes["href"].Value;
                                Console.WriteLine(u);
                            }
                        }
                    }
                }
            }
        }
    }
}