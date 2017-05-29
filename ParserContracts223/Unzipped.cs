using System;
using System.IO;
using System.IO.Compression;

namespace ParserContracts223
{
    public class Unzipped
    {
        public static string Unzip(string ar)
        {
            FileInfo fileInf = new FileInfo(ar);
            if (fileInf.Exists)
            {
                int ind = ar.LastIndexOf(".", StringComparison.Ordinal);
                string extractPath = ar.Substring(0, ind);
                try
                {
                    ZipFile.ExtractToDirectory(ar, $"{Program.Tempdir}");
                    fileInf.Delete();
                    return extractPath;
                }
                catch (Exception e)
                {
                    Log.Logger("Не удалось извлечь файл", e);
                }
            }

            return "";
        }
    }
}