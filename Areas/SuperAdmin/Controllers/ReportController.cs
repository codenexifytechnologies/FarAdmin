using AssetsManagement_Admin.Areas.SuperAdmin.Data;
using AssetsManagement_Admin.Models;
using AssetsManagement_Admin.Operations;
using ClosedXML.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AssetsManagement_Admin.Areas.SuperAdmin.Controllers
{
    public class ReportController : Controller
    {
        // GET: SuperAdmin/Report
        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login", new { area = "" });
            
            }
            ViewBag.Report = new SelectList(Common.getReportName(), "Value", "Text");
            ViewBag.Company = new SelectList(Common.getAllCompany(), "Value", "Text");
            ViewBag.VendorBranch = new SelectList(Common.getAllBranch(), "Value", "Text");
            ViewBag.Region = new SelectList(Common.getAllRegion(), "Value", "Text");
            ViewBag.State = new SelectList(Common.getAllState(), "Value", "Text");
            return View();
        }

        [HttpGet]
        public JsonResult getFilterData(int CompanyID, string BranchName, string Region, string State)
        {
            var result = Common.getFIlterData(CompanyID, BranchName, Region, State);

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetReport(int CompanyID,string BranchName)
        //{
        //    List<AssetData> objAssetData = new List<AssetData>();
        //    Response objResponse = new Response();
        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
        //            client.DefaultRequestHeaders.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            HttpResponseMessage Res = client.GetAsync(string.Format("Report/GetAllReport?CompanyId={0}&BranchName={1}", CompanyID, BranchName)).Result;
        //            if (Res.IsSuccessStatusCode)
        //            {
        //                var EmpResponse = Res.Content.ReadAsStringAsync().Result;
        //                objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);
        //                if (objResponse.StatusCode == 200)
        //                {
        //                    objAssetData = JsonConvert.DeserializeObject<List<AssetData>>(objResponse.Data.ToString());
        //                }
        //                else if (objResponse.StatusCode == 201)
        //                {
        //                    TempData["ErrorMessage"] = Convert.ToString(objResponse.Message);
        //                }
        //                else
        //                {
        //                    TempData["ErrorMessage"] = Convert.ToString(objResponse.Message);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.InnerException), "GetReport", "ReportController");
        //    }
        //    return Json(new { datas = objAssetData }, JsonRequestBehavior.AllowGet);
        //}


        public ActionResult GetReport(int CompanyID, int Report, string BranchName, string Region, string State)
        {
            Response objResponse = new Response();

            try
            {
                using (var client = new HttpClient())
                {
                    // Full FAR reports can take longer than HttpClient's default 100 seconds
                    client.Timeout = TimeSpan.FromMinutes(10);
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync(
                        string.Format("Report/GetReport?CompanyId={0}&BranchName={1}&Region={2}&State={3}&Report={4}",
                        CompanyID, BranchName, Region, State, Report)).Result;

                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);

                        if (objResponse.StatusCode == 200)
                        {

                            dynamic objAssetData = JsonConvert.DeserializeObject<dynamic>(objResponse.Data.ToString());

                            using (var workbook = new XLWorkbook())
                            {
                                var worksheet = workbook.Worksheets.Add("Report");

                                // Assuming objAssetData is a JSON array
                                JArray data = JArray.Parse(objResponse.Data.ToString());

                                // Write headers
                                var headers = ((JObject)data[0]).Properties()
                                                                .Select(p => p.Name)
                                                                .ToList();

                                for (int col = 0; col < headers.Count; col++)
                                {
                                    worksheet.Cell(1, col + 1).Value = headers[col];
                                }

                                // Write data
                                for (int row = 0; row < data.Count; row++)
                                {
                                    var item = (JObject)data[row];

                                    for (int col = 0; col < headers.Count; col++)
                                    {
                                        worksheet.Cell(row + 2, col + 1).Value =
                                            item[headers[col]]?.ToString();
                                    }
                                }

                                using (var stream = new MemoryStream())
                                {
                                    workbook.SaveAs(stream);

                                    return File(
                                        stream.ToArray(),
                                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                        "Report.xlsx");
                                }
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = Convert.ToString(objResponse.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(
                    Convert.ToString(ex.Message) + " Inner Msg:" +
                    Convert.ToString(ex.InnerException),
                    "DownloadReportCSV",
                    "ReportController");
            }

            return RedirectToAction("Index");
        }

        private string Escape(object value)
        {
            if (value == null) return "";

            string str = value.ToString();

            if (str.Contains(",") || str.Contains("\"") || str.Contains("\n"))
            {
                str = "\"" + str.Replace("\"", "\"\"") + "\"";
            }

            return str;
        }
    }
}