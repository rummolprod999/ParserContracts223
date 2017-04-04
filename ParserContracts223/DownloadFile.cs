using System;
using System.IO;
using System.Net;
using System.Web;

namespace ParserContracts223
{
    public class DownloadFile
    {
        public static string DownL(string url)
        {
            int ind = url.LastIndexOf("/");
            string namearch = url.Substring(ind + 1);
            string patharch = $"./{Program.Tempdir}/{namearch}";
            int downCount = 0;
            while (downCount >= -100)
            {
                try
                {
                    WebClient wc = new WebClient();
                    wc.DownloadFile(url, patharch);
                    return patharch;
                }
                catch (Exception e)
                {
                    FileInfo FileD = new FileInfo(patharch);
                    if (FileD.Exists)
                    {
                        FileD.Delete();
                    }
                    Log.Logger(e, url);
                }

                downCount--;
            }

            return "";
        }
    }
}