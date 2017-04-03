using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Configuration;

namespace ParserContracts223
{
    internal class Program
    {
        private static string database;
        private static string tempdir;
        private static string logdir;
        private static string suffix;
        public static string Database => database;
        public static string Tempdir => tempdir;
        public static string Logdir => logdir;
        public static string Suffix => suffix;
        private static readonly DateTime localDate = DateTime.Now;
        public static string FileLog;
        public static int add_customer = 0;
        public static int add_supplier = 0;


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
            Log.Logger("Время окончания парсинга");
            Log.Logger("Добавили customer", add_customer);
            Log.Logger("Добавили supplier", add_supplier);
        }

        private static void Init()
        {
            Setting set = new Setting();
            database = set.database;
            tempdir = set.tempdir;
            logdir = set.logdir;
            suffix = set.suffix;
            if (Directory.Exists(Tempdir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(Tempdir);
                dirInfo.Delete(true);
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