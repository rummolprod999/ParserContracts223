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
        }
    }
}