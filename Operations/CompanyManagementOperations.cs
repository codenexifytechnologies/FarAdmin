using AssetsManagement_Admin.Areas.SuperAdmin.Data;
using AssetsManagement_Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace AssetsManagement_Admin.Operations
{
    public class CompanyManagementOperations
    {
        public string RegisterCompany(Company objCompany)
        {
            string SQL_Msg = string.Empty;
            try
            {
                Response objResponse = new Response();
                using (var client = new HttpClient())
                {
                    string input = JsonConvert.SerializeObject(objCompany);
                    var content = new StringContent(input, Encoding.UTF8, "application/json");
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = client.PostAsync("Company?Operation=I", content).Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);
                        if (objResponse.StatusCode == 200)
                        {
                            List<Company> listCompany = JsonConvert.DeserializeObject<List<Company>>(objResponse.Data.ToString());
                            SQL_Msg = listCompany[0].CompanyMsg.ToString();
                        }
                    }

                    return SQL_Msg;
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "RegisterCompany", "CompanyManagementOperations");
                return SQL_Msg;
            }
        }

        public List<Company> CompanyDirectory()
        {
            List<Company> listCompany = new List<Company>();

            try
            {
                Response objResponse = new Response();

                using (var client = new HttpClient())
                {
                    var content = new StringContent("", Encoding.UTF8, "application/json");
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.PostAsync("Company?Operation=V", content).Result;

                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);

                        if (objResponse.StatusCode == 200)
                        {
                            listCompany = JsonConvert.DeserializeObject<List<Company>>(
                                objResponse.Data.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(
                    Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace),
                    "CompanyDirectory",
                    "CompanyManagementOperations");
            }

            return listCompany;
        }

        public Company CompanyDirectoryById(int Id)
        {
            Company objCompany = new Company();
            try
            {
                objCompany.CompanyId = Id;
                objCompany.CompanyName = "abc";
                objCompany.CompanyContactPerson = "abc";
                objCompany.CompanyContactEmail = "abc@abc.com";
                objCompany.CompanyContactNumber = "9999999999";

                Response objResponse = new Response();
                using (var client = new HttpClient())
                {
                    string input = JsonConvert.SerializeObject(objCompany);
                    var content = new StringContent(input, Encoding.UTF8, "application/json");
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = client.PostAsync("Company?Operation=ID", content).Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);
                        if (objResponse.StatusCode == 200)
                        {
                            objCompany = JsonConvert.DeserializeObject<List<Company>>(objResponse.Data.ToString()).FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "CompanyDirectoryById", "CompanyManagementOperations");
            }
            return objCompany;
        }

        public string EditRegisterCompany(Company objCompany)
        {
            string SQL_Msg = string.Empty;
            try
            {
                Response objResponse = new Response();
                using (var client = new HttpClient())
                {
                    string input = JsonConvert.SerializeObject(objCompany);
                    var content = new StringContent(input, Encoding.UTF8, "application/json");
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = client.PostAsync("Company?Operation=U", content).Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);
                        if (objResponse.StatusCode == 200)
                        {
                            List<Company> listCompany = JsonConvert.DeserializeObject<List<Company>>(objResponse.Data.ToString());
                            SQL_Msg = listCompany[0].CompanyMsg.ToString();
                        }
                    }

                    return SQL_Msg;
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "EditRegisterCompany", "CompanyManagementOperations");
                return SQL_Msg;
            }
        }

        public string DeleteCompany(int Id)
        {
            string SQL_Msg = string.Empty;
            try
            {
                Response objResponse = new Response();
                using (var client = new HttpClient())
                {
                    Company objCompany = new Company();
                    objCompany.CompanyId = Id;
                    objCompany.CompanyName = "abc";
                    objCompany.CompanyContactPerson= "abc";
                    objCompany.CompanyContactEmail = "abc@abc.com";
                    objCompany.CompanyContactNumber = "9999999999";


                    string input = JsonConvert.SerializeObject(objCompany);
                    var content = new StringContent(input, Encoding.UTF8, "application/json");
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    
                    HttpResponseMessage Res = client.PostAsync("Company?Operation=D", content).Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);
                        if (objResponse.StatusCode == 200)
                        {
                            List<Company> listCompany = JsonConvert.DeserializeObject<List<Company>>(objResponse.Data.ToString());
                            SQL_Msg = listCompany[0].CompanyMsg.ToString();
                        }
                    }

                    return SQL_Msg;
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "DeleteCompany", "CompanyManagementOperations");
                return SQL_Msg;
            }
        }
    }
}