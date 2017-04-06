using System;
using System.IO;
using System.Net;
using System.Web;

namespace ParserContracts223
{
    public class DownloadFile
    {
        static bool DownCount = true;

        public static string DownLOld(string url)
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

        static void DownloadF(string sSourceURL, string sDestinationPath)
        {
            long iFileSize = 0;
            int iBufferSize = 1024;
            iBufferSize *= 1000;
            long iExistLen = 0;
            FileStream saveFileStream;
            if (File.Exists(sDestinationPath))
            {
                FileInfo fINfo =
                    new FileInfo(sDestinationPath);
                iExistLen = fINfo.Length;
            }
            if (iExistLen > 0)
                saveFileStream = new FileStream(sDestinationPath,
                    FileMode.Append, FileAccess.Write,
                    FileShare.ReadWrite);
            else
                saveFileStream = new FileStream(sDestinationPath,
                    FileMode.Create, FileAccess.Write,
                    FileShare.ReadWrite);

            HttpWebRequest hwRq;
            HttpWebResponse hwRes;
            hwRq = (HttpWebRequest) HttpWebRequest.Create(sSourceURL);
            hwRq.AddRange((int) iExistLen);
            try
            {
                Stream smRespStream;
                hwRes = (HttpWebResponse) hwRq.GetResponse();
                smRespStream = hwRes.GetResponseStream();
                iFileSize = hwRes.ContentLength;
                int iByteSize;
                byte[] downBuffer = new byte[iBufferSize];

                while ((iByteSize = smRespStream.Read(downBuffer, 0, downBuffer.Length)) > 0)
                {
                    saveFileStream.Write(downBuffer, 0, iByteSize);
                }
            }
            catch (WebException ex)
            {
                hwRes = (HttpWebResponse) ex.Response;
                if (hwRes != null && (hwRes.StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable ||
                                      hwRes.StatusCode == HttpStatusCode.NotFound))
                {
                    DownCount = false;
                }
            }
        }

        public static string DownL(string url)
        {
            int ind = url.LastIndexOf("/");
            string namearch = url.Substring(ind + 1);
            string patharch = $"./{Program.Tempdir}/{namearch}";
            while (DownCount)
            {
                try
                {
                    DownloadF(url, patharch);
                }
                catch (Exception e)
                {
                    Log.Logger(e);
                }
            }

            return patharch;
        }
    }
}