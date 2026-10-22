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

namespace AssetsManagement_Admin.Operations
{
    public class VendorManagementOperations
    {
        public string RegisterVendor(Vendor objVendor)
        {
            string SQL_Msg = string.Empty;
            try
            {
                Response objResponse = new Response();
                using (var client = new HttpClient())
                {
                    string input = JsonConvert.SerializeObject(objVendor);
                    var content = new StringContent(input, Encoding.UTF8, "application/json");
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = client.PostAsync("Vendor?Operation=I", content).Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);
                        if (objResponse.StatusCode == 200)
                        {
                            List<Vendor> listVendor = JsonConvert.DeserializeObject<List<Vendor>>(objResponse.Data.ToString());
                            SQL_Msg = listVendor[0].VendorMsg.ToString();
                        }
                    }

                    return SQL_Msg;
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "RegisterVendor", "VendorManagementOperations");
                return SQL_Msg;
            }
        }

        public List<Vendor> VendorDirectory()
        {
            List<Vendor> listVendor = new List<Vendor>();

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

                    HttpResponseMessage Res = client.PostAsync("Vendor?Operation=V", content).Result;

                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);

                        if (objResponse.StatusCode == 200)
                        {
                            listVendor = JsonConvert.DeserializeObject<List<Vendor>>(
                                objResponse.Data.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(
                    Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace),
                    "VendorDirectory",
                    "VendorManagementOperations");
            }

            return listVendor;
        }

        public Vendor VendorDirectoryById(int Id)
        {
            Vendor objVendor = new Vendor();
            try
            {
                Response objResponse = new Response();
                using (var client = new HttpClient())
                {
                    objVendor.VendorId = Id;
                    objVendor.VendorName = "abc";
                    objVendor.VendorCompany = "abc";
                    objVendor.VendorBranch = "abc";
                    objVendor.VendorEmailId = "abc@abc.com";
                    objVendor.VendorContactNo = "9999999999";

                    string input = JsonConvert.SerializeObject(objVendor);
                    var content = new StringContent(input, Encoding.UTF8, "application/json");
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = client.PostAsync("Vendor?Operation=ID", content).Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);
                        if (objResponse.StatusCode == 200)
                        {
                            objVendor = JsonConvert.DeserializeObject<List<Vendor>>(objResponse.Data.ToString()).FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "VendorDirectoryById", "VendorManagementOperations");
            }
            return objVendor;
        }

        public string EditRegisterVendor(Vendor objVendor)
        {
            string SQL_Msg = string.Empty;
            try
            {
                Response objResponse = new Response();
                using (var client = new HttpClient())
                {
                    string input = JsonConvert.SerializeObject(objVendor);
                    var content = new StringContent(input, Encoding.UTF8, "application/json");
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = client.PostAsync("Vendor?Operation=U", content).Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);
                        if (objResponse.StatusCode == 200)
                        {
                            List<Vendor> listVendor = JsonConvert.DeserializeObject<List<Vendor>>(objResponse.Data.ToString());
                            SQL_Msg = listVendor[0].VendorMsg.ToString();
                        }
                    }

                    return SQL_Msg;
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "EditRegisterVendor", "VendorManagementOperations");
                return SQL_Msg;
            }
        }

        public string DeleteVendor(int Id)
        {
            string SQL_Msg = string.Empty;
            try
            {
                Response objResponse = new Response();
                using (var client = new HttpClient())
                {
                    Vendor objVendor = new Vendor();
                    objVendor.VendorId = Id;
                    objVendor.VendorName = "abc";
                    objVendor.VendorCompany = "abc";
                    objVendor.VendorBranch= "abc";
                    objVendor.VendorEmailId = "abc@abc.com";
                    objVendor.VendorContactNo = "9999999999";


                    string input = JsonConvert.SerializeObject(objVendor);
                    var content = new StringContent(input, Encoding.UTF8, "application/json");
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.PostAsync("Vendor?Operation=D", content).Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);
                        if (objResponse.StatusCode == 200)
                        {
                            List<Vendor> listVendor = JsonConvert.DeserializeObject<List<Vendor>>(objResponse.Data.ToString());
                            SQL_Msg = listVendor[0].VendorMsg.ToString();
                        }
                    }

                    return SQL_Msg;
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "DeleteVendor", "VendorManagementOperations");
                return SQL_Msg;
            }
        }

    }
}