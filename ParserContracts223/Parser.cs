using System;
using System.IO;

namespace ParserContracts223
{
    public class Parser
    {
        private readonly string _urlContract;


        public Parser(string line)
        {
            _urlContract = line;
        }

        public void Parse()
        {
            string resD = DownloadFile.DownL(_urlContract);
            if (resD == "")
            {
                Log.Logger("Не удалось получить архив за 10 попыток", _urlContract);
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

        private void Parsing(string f)
        {
            string inn,
                kpp,
                ogrn,
                regionCode,
                organizationName,
                postAddress,
                contactPhone,
                contactFax,
                contactEMail,
                lastName,
                middleName,
                firstName,
                contact_name = "";
            int contracts_count, contracts223_count = 0;
            double contracts_sum, contracts223_sum = 0.0;
        }

        private string clear_s(string s)
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
    }
}