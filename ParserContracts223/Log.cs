﻿using System;
using System.IO;

namespace ParserContracts223
{
    public class Log
    {
        public static void Logger(params object[] parametrs)
        {
            string s = "";
            s += DateTime.Now.ToString();
            for (int i = 0; i < parametrs.Length; i++)
            {
                s = $"{s} {parametrs[i]}";
            }

            using (StreamWriter sw = new StreamWriter(Program.FileLog, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(s);
            }
        }
    }
}