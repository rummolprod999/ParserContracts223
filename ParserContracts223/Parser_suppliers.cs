using System;
using System.IO;
using System.Text;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;

namespace ParserContracts223
{
    public class ParserSuppliers: IParser
    {
        private readonly string _urlSupplier;

        public ParserSuppliers(string line)
        {
            _urlSupplier = line;
        }

        public void Parse()
        {
            DownloadFile Df = new DownloadFile();
            string resD = Df.DownL(_urlSupplier);
            if (resD == "")
            {
                Log.Logger("Не удалось получить архив за 100 попыток", _urlSupplier);
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
                using (StreamReader sr = new StreamReader(file, Encoding.Default))
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
            JObject json = JObject.Parse(f);
            string kpp = (string) json.SelectToken("kpp") ?? "";
            kpp = kpp.Trim();
            int contracts223_count = (int?) json.SelectToken("contracts223Count") ?? 0;
            int contracts_count = (int?) json.SelectToken("contractsCount") ?? 0;
            double contracts223_sum = (double?) json.SelectToken("contracts223Sum") ?? 0.0;
            double contracts_sum = (double?) json.SelectToken("contractsSum") ?? 0.0;
            string ogrn = (string) json.SelectToken("ogrn") ?? "";
            string regionCode = (string) json.SelectToken("regionCode") ?? "";
            string organizationName = (string) json.SelectToken("organizationName") ?? "";
            string postAddress = (string) json.SelectToken("postAddress") ?? "";
            string contactFax = (string) json.SelectToken("contactFax") ?? "";
            string contactEMail = (string) json.SelectToken("contactEMail") ?? "";
            string contactPhone = (string) json.SelectToken("contactPhone") ?? "";
            string middleName = (string) json.SelectToken("contactInfo.middleName") ?? "";
            string firstName = (string) json.SelectToken("contactInfo.firstName") ?? "";
            string lastName = (string) json.SelectToken("contactInfo.lastName") ?? "";
            string contact_name = $"{firstName} {middleName} {lastName}";
//            Console.WriteLine(contact_name + "\n\n");
            string inn = (string) json.SelectToken("inn") ?? "";
            inn = inn.Trim();
            if (inn != "")
            {
                MySqlConnection connect =
                    ConnectToDb.GetDBConnection();
                connect.Open();
                if (kpp != "")
                {
                    string cmdSelect =
                        $"SELECT count(id) FROM od_supplier{Program.Suffix} WHERE inn = @inn AND kpp = @kpp";
                    MySqlCommand cmd = new MySqlCommand(cmdSelect, connect);
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@inn", inn);
                    cmd.Parameters.AddWithValue("@kpp", kpp);
                    int amt = 0;
                    object amtUnchecked = cmd.ExecuteScalar();
                    if (amtUnchecked != DBNull.Value && amtUnchecked != null)
                        amt = Convert.ToInt32(amtUnchecked);
                    if (amt == 0)
                    {
                        string cmdInsertWithKpp =
                            $"INSERT INTO od_supplier{Program.Suffix} SET inn = @inn, kpp = @kpp, contracts_count = @contracts_count, contracts223_count = @contracts223_count, contracts_sum = @contracts_sum, contracts223_sum = @contracts223_sum, ogrn = @ogrn,region_code = @region_code, organizationName = @organizationName,postal_address = @postal_address, contactPhone = @contactPhone,contactFax = @contactFax, contactEMail = @contactEMail,contact_name = @contact_name";
                        MySqlCommand cmdInsertKpp = new MySqlCommand(cmdInsertWithKpp, connect);
                        cmdInsertKpp.Prepare();
                        cmdInsertKpp.Parameters.AddWithValue("@inn", inn);
                        cmdInsertKpp.Parameters.AddWithValue("@kpp", kpp);
                        cmdInsertKpp.Parameters.AddWithValue("@contracts_count", contracts_count);
                        cmdInsertKpp.Parameters.AddWithValue("@contracts223_count", contracts223_count);
                        cmdInsertKpp.Parameters.AddWithValue("@contracts_sum", contracts_sum);
                        cmdInsertKpp.Parameters.AddWithValue("@contracts223_sum", contracts223_sum);
                        cmdInsertKpp.Parameters.AddWithValue("@ogrn", ogrn);
                        cmdInsertKpp.Parameters.AddWithValue("@region_code", regionCode);
                        cmdInsertKpp.Parameters.AddWithValue("@organizationName", organizationName);
                        cmdInsertKpp.Parameters.AddWithValue("@postal_address", postAddress);
                        cmdInsertKpp.Parameters.AddWithValue("@contactPhone", contactPhone);
                        cmdInsertKpp.Parameters.AddWithValue("@contactFax", contactFax);
                        cmdInsertKpp.Parameters.AddWithValue("@contactEMail", contactEMail);
                        cmdInsertKpp.Parameters.AddWithValue("@contact_name", contact_name);
                        int res_s = cmdInsertKpp.ExecuteNonQuery();
                        if (res_s > 0)
                        {
                            Program.AddSupplier++;
                        }
                        else
                        {
                            Log.Logger("Не удалось добавить supplier", _urlSupplier);
                        }
                    }
                    else
                    {
                        string cmdUpdateWithKpp =
                            $"UPDATE od_supplier{Program.Suffix} SET contracts_count = @contracts_count, contracts223_count = @contracts223_count, contracts_sum = @contracts_sum, contracts223_sum = @contracts223_sum, ogrn = @ogrn,region_code = @region_code, organizationName = @organizationName,postal_address = @postal_address, contactPhone = @contactPhone,contactFax = @contactFax, contactEMail = @contactEMail, contact_name = @contact_name WHERE inn = @inn AND kpp =@kpp";
                        MySqlCommand cmdUpdateKpp = new MySqlCommand(cmdUpdateWithKpp, connect);
                        cmdUpdateKpp.Prepare();
                        cmdUpdateKpp.Parameters.AddWithValue("@inn", inn);
                        cmdUpdateKpp.Parameters.AddWithValue("@kpp", kpp);
                        cmdUpdateKpp.Parameters.AddWithValue("@contracts_count", contracts_count);
                        cmdUpdateKpp.Parameters.AddWithValue("@contracts223_count", contracts223_count);
                        cmdUpdateKpp.Parameters.AddWithValue("@contracts_sum", contracts_sum);
                        cmdUpdateKpp.Parameters.AddWithValue("@contracts223_sum", contracts223_sum);
                        cmdUpdateKpp.Parameters.AddWithValue("@ogrn", ogrn);
                        cmdUpdateKpp.Parameters.AddWithValue("@region_code", regionCode);
                        cmdUpdateKpp.Parameters.AddWithValue("@organizationName", organizationName);
                        cmdUpdateKpp.Parameters.AddWithValue("@postal_address", postAddress);
                        cmdUpdateKpp.Parameters.AddWithValue("@contactPhone", contactPhone);
                        cmdUpdateKpp.Parameters.AddWithValue("@contactFax", contactFax);
                        cmdUpdateKpp.Parameters.AddWithValue("@contactEMail", contactEMail);
                        cmdUpdateKpp.Parameters.AddWithValue("@contact_name", contact_name);
                        int res_u = cmdUpdateKpp.ExecuteNonQuery();
                        if (res_u > 0)
                        {
                            Program.UpdateSupplier++;
                        }
                        else
                        {
                            Log.Logger("Не удалось обновить supplier", _urlSupplier);
                        }
                    }
                }
                else
                {
                    string cmdSelect =
                        $"SELECT count(id) FROM od_supplier{Program.Suffix} WHERE inn = @inn AND kpp = @kpp";
                    MySqlCommand cmd = new MySqlCommand(cmdSelect, connect);
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@inn", inn);
                    cmd.Parameters.AddWithValue("@kpp", kpp);
                    int amt = 0;
                    object amtUnchecked = cmd.ExecuteScalar();
                    if (amtUnchecked != DBNull.Value && amtUnchecked != null)
                        amt = Convert.ToInt32(amtUnchecked);
                    if (amt == 0)
                    {
                        string cmdInsertWithoutKpp =
                            $"INSERT INTO od_supplier{Program.Suffix} SET inn = @inn, kpp = @kpp, contracts_count = @contracts_count, contracts223_count = @contracts223_count, contracts_sum = @contracts_sum, contracts223_sum = @contracts223_sum, ogrn = @ogrn,region_code = @region_code, organizationName = @organizationName,postal_address = @postal_address, contactPhone = @contactPhone,contactFax = @contactFax, contactEMail = @contactEMail,contact_name = @contact_name";
                        MySqlCommand cmdInsertInn = new MySqlCommand(cmdInsertWithoutKpp, connect);
                        cmdInsertInn.Prepare();
                        cmdInsertInn.Parameters.AddWithValue("@inn", inn);
                        cmdInsertInn.Parameters.AddWithValue("@kpp", kpp);
                        cmdInsertInn.Parameters.AddWithValue("@contracts_count", contracts_count);
                        cmdInsertInn.Parameters.AddWithValue("@contracts223_count", contracts223_count);
                        cmdInsertInn.Parameters.AddWithValue("@contracts_sum", contracts_sum);
                        cmdInsertInn.Parameters.AddWithValue("@contracts223_sum", contracts223_sum);
                        cmdInsertInn.Parameters.AddWithValue("@ogrn", ogrn);
                        cmdInsertInn.Parameters.AddWithValue("@region_code", regionCode);
                        cmdInsertInn.Parameters.AddWithValue("@organizationName", organizationName);
                        cmdInsertInn.Parameters.AddWithValue("@postal_address", postAddress);
                        cmdInsertInn.Parameters.AddWithValue("@contactPhone", contactPhone);
                        cmdInsertInn.Parameters.AddWithValue("@contactFax", contactFax);
                        cmdInsertInn.Parameters.AddWithValue("@contactEMail", contactEMail);
                        cmdInsertInn.Parameters.AddWithValue("@contact_name", contact_name);
                        int res_s = cmdInsertInn.ExecuteNonQuery();
                        if (res_s > 0)
                        {
                            Program.AddSupplier++;
                        }
                        else
                        {
                            Log.Logger("Не удалось добавить supplier", _urlSupplier);
                        }
                    }
                    else
                    {
                        string cmdUpdateWithOutKpp =
                            $"UPDATE od_supplier{Program.Suffix} SET contracts_count = @contracts_count, contracts223_count = @contracts223_count, contracts_sum = @contracts_sum, contracts223_sum = @contracts223_sum, ogrn = @ogrn,region_code = @region_code, organizationName = @organizationName,postal_address = @postal_address, contactPhone = @contactPhone,contactFax = @contactFax, contactEMail = @contactEMail,contact_name = @contact_name WHERE inn = @inn AND kpp =@kpp";
                        MySqlCommand cmdUpdateInn = new MySqlCommand(cmdUpdateWithOutKpp, connect);
                        cmdUpdateInn.Prepare();
                        cmdUpdateInn.Parameters.AddWithValue("@inn", inn);
                        cmdUpdateInn.Parameters.AddWithValue("@kpp", kpp);
                        cmdUpdateInn.Parameters.AddWithValue("@contracts_count", contracts_count);
                        cmdUpdateInn.Parameters.AddWithValue("@contracts223_count", contracts223_count);
                        cmdUpdateInn.Parameters.AddWithValue("@contracts_sum", contracts_sum);
                        cmdUpdateInn.Parameters.AddWithValue("@contracts223_sum", contracts223_sum);
                        cmdUpdateInn.Parameters.AddWithValue("@ogrn", ogrn);
                        cmdUpdateInn.Parameters.AddWithValue("@region_code", regionCode);
                        cmdUpdateInn.Parameters.AddWithValue("@organizationName", organizationName);
                        cmdUpdateInn.Parameters.AddWithValue("@postal_address", postAddress);
                        cmdUpdateInn.Parameters.AddWithValue("@contactPhone", contactPhone);
                        cmdUpdateInn.Parameters.AddWithValue("@contactFax", contactFax);
                        cmdUpdateInn.Parameters.AddWithValue("@contactEMail", contactEMail);
                        cmdUpdateInn.Parameters.AddWithValue("@contact_name", contact_name);
                        int res_u = cmdUpdateInn.ExecuteNonQuery();
                        if (res_u > 0)
                        {
                            Program.UpdateSupplier++;
                        }
                        else
                        {
                            Log.Logger("Не удалось обновить supplier", _urlSupplier);
                        }
                    }
                }


                connect.Close();
                connect = null;
            }
            else
            {
                Program.InnNullSupplier++;
            }
        }
    }
}