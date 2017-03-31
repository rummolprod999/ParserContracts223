using System;

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

        }
    }
}