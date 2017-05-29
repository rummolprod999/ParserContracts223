using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace ParserContracts223
{
    public class FileList
    {
        private static string _urlClearspending = "https://clearspending.ru/opendata/";

        public static List<string> GetUrl(string findS)
        {
            List<string> urls = new List<string>();
            string[] years = new[] {"_2015", "_2016", "_2017"};

            var request = WebRequest.Create(_urlClearspending);
            using (var responses = request.GetResponse())
            {
                using (var streams = responses.GetResponseStream())
                using (var readers = new StreamReader(streams))
                {
                    string html = readers.ReadToEnd();
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    HtmlNodeCollection c =
                        doc.DocumentNode.SelectNodes($"//a[contains(@href,'{findS}')]");
                    if (c != null)
                    {
                        foreach (HtmlNode n in c)
                        {
                            if (n.Attributes["href"] != null)
                            {
                                string u = n.Attributes["href"].Value;
                                if (Program.Typeparsing == TypeArgument.Contr223)
                                {
                                    if (years.Any(t => u.IndexOf(t, StringComparison.Ordinal) != -1))
                                    {
                                        u = $"https://clearspending.ru{u}";
                                        urls.Add(u);
                                    }
                                }
                                else
                                {
                                    u = $"https://clearspending.ru{u}";
                                    urls.Add(u);
                                }
                            }
                        }
                    }
                }
            }

            return urls;
        }
    }
}