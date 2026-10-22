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
    public class ReportOpertaion
    {
        public List<AssetData> GetReport(int CompanyID, string BranchName)
        {
            List<AssetData> listAssetData = new List<AssetData>();
            try
            {
                Response objResponse = new Response();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = client.GetAsync("Report/GetAllReport?CompanyId="+ CompanyID + "&BranchName="+ BranchName).Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);
                        if (objResponse.StatusCode == 200)
                        {
                            listAssetData = JsonConvert.DeserializeObject<List<AssetData>>(objResponse.Data.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "GetReport", "ReportOperation");
            }
            return listAssetData;
        }
    }
}