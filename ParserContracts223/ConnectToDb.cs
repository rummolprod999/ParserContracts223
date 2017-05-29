using System;
using MySql.Data.MySqlClient;

namespace ParserContracts223
{
    public class ConnectToDb
    {
        public static MySqlConnection
            GetDBConnection()
        {
            // Connection String.
            String connString = "Server=" + Program.Server + ";Port=" + Program.Port + ";Database=" + Program.Database
                                + ";User Id=" + Program.User + ";password=" + Program.Pass + ";CharSet=utf8";

            MySqlConnection conn = new MySqlConnection(connString);

            return conn;
        }
    }
}