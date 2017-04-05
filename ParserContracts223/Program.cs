using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Configuration;

namespace ParserContracts223
{
    internal class Program
    {
        private static string _database;
        private static string _tempdir;
        private static string _logdir;
        private static string _suffix;
        private static string _user;
        private static string _pass;
        public static string Database => _database;
        public static string Tempdir => _tempdir;
        public static string Logdir => _logdir;
        public static string Suffix => _suffix;
        public static string User => _user;
        public static string Pass => _pass;
        private static readonly DateTime LocalDate = DateTime.Now;
        public static string FileLog;
        public static int AddCustomer = 0;
        public static int AddSupplier = 0;
        public static int UpdateSupplier = 0;
        public static int InnNullSupplier = 0;
        public static int UpdateCustomer = 0;
        public static int RegnumNullCustomer = 0;


        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Недостаточно аргументов для запуска, используйте customer, supplier или contr223");
                return;
            }


            switch (args[0])
            {
                case "contr223":
                    Init("contr223");
                    Pars_contr223();
                    break;
                case "supplier":
                    Init("suppliers");
                    Pars_suppliers();
                    break;
                case "customer":
                    Init("customers");
                    Pars_customers();
                    break;
                default:
                    Console.WriteLine("Используйте customer, supplier или contr223 в качестве аргументов");
                    break;
            }
        }

        private static void Init(string arg)
        {
            Setting set = new Setting();
            _database = set.Database;
            _logdir = set.Logdir;
            _suffix = set.Suffix;
            _user = set.User;
            _pass = set.Pass;
            switch (arg)
            {
                case "contr223":
                    _tempdir = set.TempdirContract223;
                    break;
                case "suppliers":
                    _tempdir = set.TempdirSupplier;
                    break;
                case "customers":
                    _tempdir = set.TempdirCustomer;
                    break;
            }
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
            if (!Directory.Exists(Logdir))
            {
                Directory.CreateDirectory(Logdir);
            }
            FileLog = $"./{Logdir}/{arg}_{LocalDate:dd_MM_yyyy}.log";
        }

        private static void Pars_contr223()
        {
            Log.Logger("Время начала парсинга contracts223");
            List<string> Listurl = FileList.GetUrl("/download/opendata/contracts_223fz");
            foreach (var l in Listurl)
            {
                ParserContract223 p = new ParserContract223(l);
                p.Parse();
            }

            Log.Logger("Время окончания парсинга");
            Log.Logger("Добавили customer по contracts223", AddCustomer);
            Log.Logger("Добавили supplier по contracts223", AddSupplier);
        }

        private static void Pars_suppliers()
        {
            Log.Logger("Время начала парсинга suppliers");
            List<string> Listurl = FileList.GetUrl("/download/opendata/suppliers-");
            foreach (var l in Listurl)
            {
                ParserSuppliers p = new ParserSuppliers(l);
                p.Parse();
            }

            Log.Logger("Время окончания парсинга");
            Log.Logger("Добавили supplier", AddSupplier);
            Log.Logger("Обновили supplier", UpdateSupplier);
            Log.Logger("supplier без инн", InnNullSupplier);
        }

        private static void Pars_customers()
        {
            Log.Logger("Время начала парсинга customers");
            List<string> Listurl = FileList.GetUrl("/download/opendata/customers-");
            foreach (var l in Listurl)
            {
                ParserCustomers p = new ParserCustomers(l);
                p.Parse();
            }

            Log.Logger("Время окончания парсинга");
            Log.Logger("Добавили customer", AddCustomer);
            Log.Logger("Обновили customer", UpdateCustomer);
            Log.Logger("customer без RegNum", RegnumNullCustomer);
        }
    }
}