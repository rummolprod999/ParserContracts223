using System;
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
            int downCount = 10;
            while (downCount >= -10)
            {
                try
                {
                    WebClient wc = new WebClient();
                    wc.DownloadFile(url, patharch);
                    return patharch;
                }
                catch (Exception e)
                {
                    Log.Logger(e);
                }

                downCount--;
            }

            return "";
        }
    }
}