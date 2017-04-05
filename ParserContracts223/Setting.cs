using System;
using System.Xml;
namespace ParserContracts223
{
    public class Setting
    {
        public readonly string Database;
        public readonly string TempdirContract223;
        public readonly string TempdirSupplier;
        public readonly string TempdirCustomer;
        public readonly string Logdir;
        public readonly string Suffix;
        public readonly string User;
        public readonly string Pass;

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
                        Database = xnode.InnerText;
                        break;
                    case "temp_contract223":
                        TempdirContract223 = xnode.InnerText;
                        break;
                    case "temp_customer":
                        TempdirCustomer = xnode.InnerText;
                        break;
                    case "temp_supplier":
                        TempdirSupplier = xnode.InnerText;
                        break;
                    case "logdir":
                        Logdir = xnode.InnerText;
                        break;
                    case "suffix":
                        Suffix = xnode.InnerText;
                        break;
                    case "user":
                        User = xnode.InnerText;
                        break;
                    case "pass":
                        Pass = xnode.InnerText;
                        break;
                }
            }
        }

    }
}