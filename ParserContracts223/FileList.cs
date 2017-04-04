using System;
using System.IO;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Collections;
using System.Collections.Generic;

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
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
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
                                for (int i = 0; i < years.Length; i++)
                                {
                                    if (u.IndexOf(years[i], StringComparison.Ordinal) != -1)
                                    {
                                        u = $"https://clearspending.ru{u}";
                                        urls.Add(u);
                                        break;
                                    }
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