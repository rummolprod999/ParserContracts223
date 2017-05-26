using System;
using System.Xml;
using System.IO;
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
        public readonly string Server;
        public readonly int Port;

        public Setting()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Program.PathProgram + Path.DirectorySeparatorChar + "setting.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                switch (xnode.Name)
                {
                case "datebase":
                        Database = xnode.InnerText;
                        break;
                    case "tempdir_contract223":
                        TempdirContract223 = $"{Program.PathProgram}{Path.DirectorySeparatorChar}{xnode.InnerText}";
                        break;
                    case "tempdir_customer":
                        TempdirCustomer = $"{Program.PathProgram}{Path.DirectorySeparatorChar}{xnode.InnerText}";
                        break;
                    case "tempdir_supplier":
                        TempdirSupplier = $"{Program.PathProgram}{Path.DirectorySeparatorChar}{xnode.InnerText}";
                        break;
                    case "logdir":
                        Logdir = $"{Program.PathProgram}{Path.DirectorySeparatorChar}{xnode.InnerText}";
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
                    case "server":
                        Server = xnode.InnerText;
                        break;
                    case "port":
                        Port = Int32.Parse(xnode.InnerText);
                        break;
                }
            }
        }

    }
}