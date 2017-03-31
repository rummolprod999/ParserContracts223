using System;
using System.Xml;
namespace ParserContracts223
{
    public class Setting
    {
        public readonly string database;
        public readonly string tempdir;
        public readonly string logdir;

        public Setting()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("setting.xml");
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                if(xnode.Name=="datebase")
                    {
                        database = xnode.InnerText;
                    }
                if(xnode.Name=="tempdir")
                {
                    tempdir = xnode.InnerText;
                }
                if(xnode.Name=="logdir")
                {
                    logdir = xnode.InnerText;
                }

            }
        }

    }
}