using System;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;

namespace ParserContracts223
{
    public class ParserCustomers: IParser
    {
        private readonly string _urlCustomer;

        public ParserCustomers(string line)
        {
            _urlCustomer = line;
        }

        public void Parse()
        {
            string resD = DownloadFile.DownL(_urlCustomer);
            if (resD == "")
            {
                Log.Logger("Не удалось получить архив за 100 попыток", _urlCustomer);
                return;
            }

            string file = Unzipped.Unzip(resD);
            if (file == "")
            {
                Log.Logger("Не разархивировали файл", resD);
                return;
            }
            FileInfo fileInf = new FileInfo(file);
            if (fileInf.Exists)
            {
                using (StreamReader sr = new StreamReader(file, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = clear_s(line);
                        try
                        {
                            Parsing(line);
                        }
                        catch (Exception e)
                        {
                            Log.Logger(e);
                        }
                    }
                }

                fileInf.Delete();
            }
        }

        public string clear_s(string s)
        {
            string st = s;
            st = st.Trim();
            if (st.StartsWith("["))
            {
                st = st.Remove(0, 1);
            }
            if (st.IndexOf(',', (st.Length - 1)) != -1)
            {
                st = st.Remove(st.Length - 1);
            }
            if (st.IndexOf(']', (st.Length - 1)) != -1)
            {
                st = st.Remove(st.Length - 1);
            }

            return st;
        }

        public void Parsing(string f)
        {
        }
    }
}