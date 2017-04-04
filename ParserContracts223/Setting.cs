using System;
using System.Xml;
namespace ParserContracts223
{
    public class Setting
    {
        public readonly string database;
        public readonly string tempdir;
        public readonly string logdir;
        public readonly string suffix;
        public readonly string user;
        public readonly string pass;

        public Setting()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("setting.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                switch (xnode.Name)
                {
                    case "datebase":
                        database = xnode.InnerText;
                        break;
                    case "tempdir":
                        tempdir = xnode.InnerText;
                        break;
                    case "logdir":
                        logdir = xnode.InnerText;
                        break;
                    case "suffix":
                        suffix = xnode.InnerText;
                        break;
                    case "user":
                        user = xnode.InnerText;
                        break;
                    case "pass":
                        pass = xnode.InnerText;
                        break;
                }
            }
        }

    }
}