using System;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;

namespace ParserContracts223
{
    public class ParserCustomers : IParser
    {
        private readonly string _urlCustomer;

        public ParserCustomers(string line)
        {
            _urlCustomer = line;
        }

        public void Parse()
        {
            DownloadFile Df = new DownloadFile();
            string resD = Df.DownL(_urlCustomer);
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
                        line = Tools.ClearString(line);
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


        public void Parsing(string f)
        {
            MySqlConnection connect =
                ConnectToDb.GetDBConnection("localhost", Program.Database, Program.User, Program.Pass);
            connect.Open();
            JObject json = JObject.Parse(f);
            string customer_regnumber = (string) json.SelectToken("regNumber") ?? "";
            customer_regnumber = customer_regnumber.Trim();
            if (customer_regnumber != "")
            {
                string kpp_customer = (string) json.SelectToken("kpp") ?? "";
                int contracts223_count_customer = (int?) json.SelectToken("contracts223Count") ?? 0;
                int contracts_count_customer = (int?) json.SelectToken("contractsCount") ?? 0;
                double contracts223_sum_customer = (double?) json.SelectToken("contracts223Sum") ?? 0.0;
                double contracts_sum_customer = (double?) json.SelectToken("contractsSum") ?? 0.0;
                string ogrn_customer = (string) json.SelectToken("ogrn") ?? "";
                string region_code_customer = (string) json.SelectToken("regionCode") ?? "";
                string full_name_customer = (string) json.SelectToken("fullName") ?? "";
                string fax_customer = (string) json.SelectToken("fax") ?? "";
                string email_customer = (string) json.SelectToken("url") ?? "не указан";
                string phone_customer = (string) json.SelectToken("phone") ?? "";
                string middlename = (string) json.SelectToken("contactPerson.middleName") ?? "";
                string firstname = (string) json.SelectToken("contactPerson.firstName") ?? "";
                string lastname = (string) json.SelectToken("contactPerson.lastName") ?? "";
                string contact_name_customer = $"{firstname} {middlename} {lastname}";
                string inn_customer = (string) json.SelectToken("inn") ?? "";
                string postal_address_customer = (string) json.SelectToken("postalAddress") ?? "";
                string select_customer =
                    $"SELECT id FROM od_customer{Program.Suffix} WHERE regNumber = @customer_regnumber";
                MySqlCommand cmd = new MySqlCommand(select_customer, connect);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@customer_regnumber", customer_regnumber);
                MySqlDataReader reader = cmd.ExecuteReader();
                bool res_read = reader.HasRows;
                reader.Close();
                if (!res_read)
                {
                    string add_customer =
                        $"INSERT INTO od_customer{Program.Suffix} SET regNumber = @customer_regnumber, inn = @inn_customer, " +
                        $"kpp = @kpp_customer, contracts_count = @contracts_count_customer, contracts223_count = @contracts223_count_customer," +
                        $"contracts_sum = @contracts_sum_customer, contracts223_sum = @contracts223_sum_customer," +
                        $"ogrn = @ogrn_customer, region_code = @region_code_customer, full_name = @full_name_customer," +
                        $"postal_address = @postal_address_customer, phone = @phone_customer, fax = @fax_customer," +
                        $"email = @email_customer, contact_name = @contact_name_customer";
                    MySqlCommand cmd2 = new MySqlCommand(add_customer, connect);
                    cmd2.Prepare();
                    cmd2.Parameters.AddWithValue("@customer_regnumber", customer_regnumber);
                    cmd2.Parameters.AddWithValue("@inn_customer", inn_customer);
                    cmd2.Parameters.AddWithValue("@kpp_customer", kpp_customer);
                    cmd2.Parameters.AddWithValue("@contracts_count_customer", contracts_count_customer);
                    cmd2.Parameters.AddWithValue("@contracts223_count_customer", contracts223_count_customer);
                    cmd2.Parameters.AddWithValue("@contracts_sum_customer", contracts_sum_customer);
                    cmd2.Parameters.AddWithValue("@contracts223_sum_customer", contracts223_sum_customer);
                    cmd2.Parameters.AddWithValue("@ogrn_customer", ogrn_customer);
                    cmd2.Parameters.AddWithValue("@region_code_customer", region_code_customer);
                    cmd2.Parameters.AddWithValue("@full_name_customer", full_name_customer);
                    cmd2.Parameters.AddWithValue("@postal_address_customer", postal_address_customer);
                    cmd2.Parameters.AddWithValue("@phone_customer", phone_customer);
                    cmd2.Parameters.AddWithValue("@fax_customer", fax_customer);
                    cmd2.Parameters.AddWithValue("@email_customer", email_customer);
                    cmd2.Parameters.AddWithValue("@contact_name_customer", contact_name_customer);
                    int add_c = cmd2.ExecuteNonQuery();
                    if (add_c > 0)
                    {
                        Program.AddCustomer++;
                    }
                    else
                    {
                        Log.Logger("Не удалось добавить customer", _urlCustomer);
                    }
                }
                else
                {
                    string update_customer =
                        $"UPDATE od_customer{Program.Suffix} SET inn = @inn_customer, " +
                        $"kpp = @kpp_customer, contracts_count = @contracts_count_customer, contracts223_count = @contracts223_count_customer," +
                        $"contracts_sum = @contracts_sum_customer, contracts223_sum = @contracts223_sum_customer," +
                        $"ogrn = @ogrn_customer, region_code = @region_code_customer, full_name = @full_name_customer," +
                        $"postal_address = @postal_address_customer, phone = @phone_customer, fax = @fax_customer," +
                        $"email = @email_customer, contact_name = @contact_name_customer WHERE regNumber = @customer_regnumber";
                    MySqlCommand cmd3 = new MySqlCommand(update_customer, connect);
                    cmd3.Prepare();
                    cmd3.Parameters.AddWithValue("@customer_regnumber", customer_regnumber);
                    cmd3.Parameters.AddWithValue("@inn_customer", inn_customer);
                    cmd3.Parameters.AddWithValue("@kpp_customer", kpp_customer);
                    cmd3.Parameters.AddWithValue("@contracts_count_customer", contracts_count_customer);
                    cmd3.Parameters.AddWithValue("@contracts223_count_customer", contracts223_count_customer);
                    cmd3.Parameters.AddWithValue("@contracts_sum_customer", contracts_sum_customer);
                    cmd3.Parameters.AddWithValue("@contracts223_sum_customer", contracts223_sum_customer);
                    cmd3.Parameters.AddWithValue("@ogrn_customer", ogrn_customer);
                    cmd3.Parameters.AddWithValue("@region_code_customer", region_code_customer);
                    cmd3.Parameters.AddWithValue("@full_name_customer", full_name_customer);
                    cmd3.Parameters.AddWithValue("@postal_address_customer", postal_address_customer);
                    cmd3.Parameters.AddWithValue("@phone_customer", phone_customer);
                    cmd3.Parameters.AddWithValue("@fax_customer", fax_customer);
                    cmd3.Parameters.AddWithValue("@email_customer", email_customer);
                    cmd3.Parameters.AddWithValue("@contact_name_customer", contact_name_customer);
                    int add_c = cmd3.ExecuteNonQuery();
                    if (add_c > 0)
                    {
                        Program.UpdateCustomer++;
                    }
                    else
                    {
                        Log.Logger("Не удалось обновить customer", _urlCustomer);
                    }
                }
            }
            else
            {
                Program.RegnumNullCustomer++;
            }
            connect.Close();
            connect = null;
        }
    }
}