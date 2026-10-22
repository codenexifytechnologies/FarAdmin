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
    public class AssetOperations
    {
        public List<AssetData> GetAssetList(FilterRequest objFilterRequest)
        {
            List<AssetData> listAssetData = new List<AssetData>();

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

                    HttpResponseMessage Res = client.GetAsync("GetFARData?CompanyId=" + objFilterRequest.CompanyId + "&BranchName=" + objFilterRequest.BranchName).Result;

                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);

                        if (objResponse.StatusCode == 200)
                        {
                            listAssetData = JsonConvert.DeserializeObject<List<AssetData>>(
                                objResponse.Data.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(
                    Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace),
                    "GetAssetList",
                    "AssetOperations");
            }

            return listAssetData;
        }

        public AssetData GetAssetListById(int Id)
        {
            AssetData objAssetData = new AssetData();

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

                    HttpResponseMessage Res = client.GetAsync("GetFARDataById?Id=" + Id).Result;

                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);

                        if (objResponse.StatusCode == 200)
                        {
                            objAssetData = JsonConvert.DeserializeObject<AssetData>(
                                objResponse.Data.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(
                    Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace),
                    "GetAssetListById",
                    "AssetOperations");
            }

            return objAssetData;
        }

        public string UpdateAsset(AssetData objAssetData)
        {
            string Result=null;
            try
            {
                Response objResponse = new Response();

                using (var client = new HttpClient())
                {
                    string input = JsonConvert.SerializeObject(objAssetData);
                    var content = new StringContent(input, Encoding.UTF8, "application/json");
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = client.PostAsync("UpdateFARData", content).Result;

                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);

                        if (objResponse.StatusCode == 200)
                        {
                            Result = JsonConvert.DeserializeObject<string>(
                                objResponse.Data.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(
                    Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace),
                    "UpdateAsset",
                    "AssetOperations");
            }

            return Result;
        }
        

    }
}