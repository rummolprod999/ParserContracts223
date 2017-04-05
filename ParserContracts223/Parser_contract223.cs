using System;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;

namespace ParserContracts223
{
    public class ParserContract223 : IParser
    {
        private readonly string _urlContract;


        public ParserContract223(string line)
        {
            _urlContract = line;
        }

        public void Parse()
        {
            string resD = DownloadFile.DownL(_urlContract);
            if (resD == "")
            {
                Log.Logger("Не удалось получить архив за 100 попыток", _urlContract);
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
            double contract_price = (double?) json.SelectToken("price") ?? 0.0;
            string customer_regnumber = (string) json.SelectToken("customer.regNum") ?? "";
            customer_regnumber = customer_regnumber.Trim();
            if (customer_regnumber != "")
            {
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
                    string kpp_customer = (string) json.SelectToken("customer.kpp") ?? "";
                    string full_name_customer = (string) json.SelectToken("customer.fullName") ?? "";
                    string inn_customer = (string) json.SelectToken("customer.inn") ?? "";
                    string postal_address_customer = (string) json.SelectToken("customer.postalAddress") ?? "";
                    string fax_customer = (string) json.SelectToken("customer.fax") ?? "";
                    string ogrn_customer = (string) json.SelectToken("customer.OGRN") ?? "";
                    string phone_customer = (string) json.SelectToken("customer.phone") ?? "";
                    string email_customer = (string) json.SelectToken("customer.email") ?? "";
                    int contracts223_count_customer = 1;
                    double contracts223_sum_customer = contract_price;
                    int contracts_count_customer = 0;
                    double contracts_sum_customer = 0.0;
                    string region_code_customer = "";
                    string contact_name_customer = "";
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
                        Log.Logger("Не удалось добавить customer", _urlContract);
                    }
                }
            }
            var test_sup = json.SelectToken("suppliers");
            if (test_sup != null && test_sup.Type != JTokenType.Null)
            {
                var suppliers = from s in json["suppliers"] select s;
                foreach (var supplier in suppliers)
                {
                    /*Console.WriteLine(supplier.ToString());*/
                    string supplier_inn = (string) supplier.SelectToken("inn") ?? "";

                    supplier_inn = supplier_inn.Trim();
                    string kpp_supplier = (string) supplier.SelectToken("kpp") ?? "";
                    kpp_supplier = kpp_supplier.Trim();

                    if (supplier_inn != "")
                    {
                        string select_supplier =
                            $"SELECT id FROM od_supplier{Program.Suffix} WHERE inn = @supplier_inn AND kpp = @kpp_supplier";
                        MySqlCommand cmd = new MySqlCommand(select_supplier, connect);
                        cmd.Prepare();
                        cmd.Parameters.AddWithValue("@supplier_inn", supplier_inn);
                        cmd.Parameters.AddWithValue("@kpp_supplier", kpp_supplier);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        bool res_read = reader.HasRows;
                        reader.Close();
                        if (!res_read)
                        {
                            string contactphone_supplier = (string) supplier.SelectToken("contactPhone") ?? "";
                            string contactemail_supplier = (string) supplier.SelectToken("contactEMail") ?? "";
                            string organizationname_supplier = (string) supplier.SelectToken("organizationName") ?? "";
                            /*Console.WriteLine(organizationname_supplier);*/
                            string orgn_supplier = (string) supplier.SelectToken("ogrn") ?? "";
                            int contracts_count_supplier = 0;
                            int contracts223_count_supplier = 1;
                            double contracts_sum_supplier = 0.0;
                            double contracts223_sum_supplier = contract_price;
                            string region_code_supplier = "";
                            string post_address_supplier = "";
                            string contact_fax_supplier = "";
                            string contact_name_supplier = "";
                            string add_supplier =
                                $"INSERT INTO od_supplier{Program.Suffix} SET inn = @supplier_inn, kpp = @kpp_supplier, " +
                                $"contracts_count = @contracts_count, " +
                                $"contracts223_count = @contracts223_count, contracts_sum = @contracts_sum, " +
                                $"contracts223_sum = @contracts223_sum, ogrn = @ogrn,region_code = @region_code, " +
                                $"organizationName = @organizationName,postal_address = @postal_address, " +
                                $"contactPhone = @contactPhone,contactFax = @contactFax, " +
                                $"contactEMail = @contactEMail,contact_name = @contact_name";
                            MySqlCommand cmd3 = new MySqlCommand(add_supplier, connect);
                            cmd3.Prepare();
                            cmd3.Parameters.AddWithValue("@supplier_inn", supplier_inn);
                            cmd3.Parameters.AddWithValue("@kpp_supplier", kpp_supplier);
                            cmd3.Parameters.AddWithValue("@contracts_count", contracts_count_supplier);
                            cmd3.Parameters.AddWithValue("@contracts223_count", contracts223_count_supplier);
                            cmd3.Parameters.AddWithValue("@contracts_sum", contracts_sum_supplier);
                            cmd3.Parameters.AddWithValue("@contracts223_sum", contracts223_sum_supplier);
                            cmd3.Parameters.AddWithValue("@ogrn", orgn_supplier);
                            cmd3.Parameters.AddWithValue("@region_code", region_code_supplier);
                            cmd3.Parameters.AddWithValue("@organizationName", organizationname_supplier);
                            cmd3.Parameters.AddWithValue("@postal_address", post_address_supplier);
                            cmd3.Parameters.AddWithValue("@contactPhone", contactphone_supplier);
                            cmd3.Parameters.AddWithValue("@contactFax", contact_fax_supplier);
                            cmd3.Parameters.AddWithValue("@contactEMail", contactemail_supplier);
                            cmd3.Parameters.AddWithValue("@contact_name", contact_name_supplier);
                            int add_s = cmd3.ExecuteNonQuery();
                            if (add_s > 0)
                            {
                                Program.AddSupplier++;
                            }
                            else
                            {
                                Log.Logger("Не удалось добавить supplier", _urlContract);
                            }
                        }
                    }
                }
            }


            connect.Close();
            connect = null;
        }
    }
}