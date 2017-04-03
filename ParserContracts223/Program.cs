using System;
using System.Collections.Generic;
using System.IO;

namespace ParserContracts223
{
    internal class Program
    {
        private static string database;
        private static string tempdir;
        private static string logdir;
        public static string Database => database;
        public static string Tempdir => tempdir;
        public static string Logdir => logdir;
        private static readonly DateTime localDate = DateTime.Now;
        public static string FileLog;


        public static void Main(string[] args)
        {
            Init();
            Log.Logger("Время начала парсинга");
            List<string> Listurl = FileList.GetUrl();
            foreach (var l in Listurl)
            {
                Parser p = new Parser(l);
                p.Parse();
            }
        }

        private static void Init()
        {
            Setting set = new Setting();
            database = set.database;
            tempdir = set.tempdir;
            logdir = set.logdir;
            if (Directory.Exists(Tempdir))
            {
                Directory.Delete(Tempdir);
                Directory.CreateDirectory(Tempdir);
            }
            else
            {
                Directory.CreateDirectory(Tempdir);
            }
            FileLog = $"./{Logdir}/contracts223_{localDate:dd_MM_yyyy}.log";
        }
    }
}