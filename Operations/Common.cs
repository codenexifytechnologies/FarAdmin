using AssetsManagement_Admin.Areas.SuperAdmin.Data;
using AssetsManagement_Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AssetsManagement_Admin.Operations
{
    public class CBranch
    {
        public int Id { get; set; }
        public string Location { get; set; }
    }

    public class CRegion
    {
        public int Id { get; set; }
        public string Region { get; set; }
    }

    public class CState
    {
        public int Id { get; set; }
        public string State { get; set; }
    }

    public class CReport
    {
        public int Id { get; set; }
        public string ReportName { get; set; }
    }

    public class FilterRequest
    {
        public int CompanyId { get; set; }
        public string BranchName { get; set; }
        public string Region { get; set; }
        public string State { get; set; }

    }
    public class Common
    {
        private static readonly object LogLock = new object();

        // Logging must never throw: concurrent File.AppendAllText calls on the same file fail with IOException
        private static void AppendLog(string fileName, string contents)
        {
            try
            {
                string Path = HttpContext.Current != null
                    ? HttpContext.Current.Server.MapPath("~/" + fileName)
                    : System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                lock (LogLock)
                {
                    File.AppendAllText(Path, contents);
                }
            }
            catch
            {
            }
        }

        public static void errorlog(string error, string method, string controller)
        {
            string Contents = "\n DateTime: " + DateTime.Now + "\n===========================\n Method Name: " + method + "\n Error: \n" + error + "\n controller: \n" + controller;
            AppendLog("Error.txt", Contents);
        }

        public static void SaveRequestResponse(string MethodName, string Request, string Response)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["SaveRequest"]))
            {
                string Contents = "\n DateTime: " + DateTime.Now + "\n===========================\n Method Name: " + MethodName + "\n Request: \n" + Request + "\n Response: \n" + Response;
                AppendLog("RequestResponse.txt", Contents);
            }
        }

        public static List<List<SelectListItem>> getFIlterData(int CompanyID, string BranchName, string Region, string State)
        {
            List<List<SelectListItem>> obj = new List<List<SelectListItem>>();
            List<SelectListItem> lstBranch = new List<SelectListItem>();
            List<SelectListItem> lstRegion = new List<SelectListItem>();
            List<SelectListItem> lstState = new List<SelectListItem>();

            Response objResponse = new Response();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync($"Report/GetFilterData?CompanyID={CompanyID}&BranchName={BranchName}&Region={Region}&State={State}").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);
                        if (objResponse.StatusCode == 200)
                        {
                            var listData = JsonConvert.DeserializeObject<List<AssetData>>(objResponse.Data.ToString());
                            lstBranch = listData.Where(x => !string.IsNullOrEmpty(x.LOCATION)).Select(x => x.LOCATION).Distinct().Select(x => new SelectListItem{Text = x,Value = x}).ToList();
                            lstRegion = listData.Where(x => !string.IsNullOrEmpty(x.Region)).Select(x => x.Region).Distinct().Select(x => new SelectListItem { Text = x, Value = x }).ToList();
                            lstState = listData.Where(x => !string.IsNullOrEmpty(x.State)).Select(x => x.State).Distinct().Select(x => new SelectListItem { Text = x, Value = x }).ToList();
                            obj.Add(lstBranch);
                            obj.Add(lstRegion);
                            obj.Add(lstState);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Common.errorlog("getBranch", Ex.Message.ToString(), Convert.ToString(Ex.InnerException));
            }

            return obj;
        }

        public static List<SelectListItem> getReportName()
        {
            List<SelectListItem> lstReport = new List<SelectListItem>();
            Response objResponse = new Response();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync("Report/GetReportName").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);
                        if (objResponse.StatusCode == 200)
                        {
                            var listReport = JsonConvert.DeserializeObject<List<CReport>>(objResponse.Data.ToString());
                            lstReport = listReport.Select(x => new SelectListItem { Text = x.ReportName, Value = Convert.ToString(x.Id) }).ToList();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Common.errorlog("getReportName", Ex.Message.ToString(), Convert.ToString(Ex.InnerException));
            }

            return lstReport;
        }

        public static List<SelectListItem> getAllCompany()
        {
            List<SelectListItem> lstCompany = new List<SelectListItem>();
            Response objResponse = new Response();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync("GetCompany").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);
                        if (objResponse.StatusCode == 200)
                        {
                            var listCompany = JsonConvert.DeserializeObject<List<Company>>(objResponse.Data.ToString());
                            lstCompany = listCompany.Select(x => new SelectListItem { Text = x.CompanyName, Value = Convert.ToString(x.CompanyId) }).ToList();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Common.errorlog("getCompany", Ex.Message.ToString(), Convert.ToString(Ex.InnerException));
            }

            return lstCompany;
        }

        public static List<SelectListItem> getAllBranch()
        {
            List<SelectListItem> lstAllBranch = new List<SelectListItem>();
            Response objResponse = new Response();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync("Report/GetAllBranch").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);
                        if (objResponse.StatusCode == 200)
                        {
                            var listAllBranch = JsonConvert.DeserializeObject<List<CBranch>>(objResponse.Data.ToString());
                            lstAllBranch = listAllBranch.Select(x => new SelectListItem { Text = x.Location, Value = x.Location }).ToList();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Common.errorlog("getAllBranch", Ex.Message.ToString(), Convert.ToString(Ex.InnerException));
            }

            return lstAllBranch;
        }

        public static List<SelectListItem> getAllRegion()
        {
            List<SelectListItem> lstRegion = new List<SelectListItem>();
            Response objResponse = new Response();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync("Report/GetAllRegion").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var Response = Res.Content.ReadAsStringAsync().Result;
                        objResponse = JsonConvert.DeserializeObject<Response>(Response);
                        if (objResponse.StatusCode == 200)
                        {
                            var listRegion = JsonConvert.DeserializeObject<List<CRegion>>(objResponse.Data.ToString());
                            lstRegion = listRegion.Select(x => new SelectListItem { Text = x.Region, Value = x.Region }).ToList();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Common.errorlog("getAllRegion", Ex.Message.ToString(), Convert.ToString(Ex.InnerException));
            }

            return lstRegion;
        }

        public static List<SelectListItem> getAllState()
        {
            List<SelectListItem> lstState = new List<SelectListItem>();
            Response objResponse = new Response();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync("Report/GetAllState").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var Response = Res.Content.ReadAsStringAsync().Result;
                        objResponse = JsonConvert.DeserializeObject<Response>(Response);
                        if (objResponse.StatusCode == 200)
                        {
                            var listState = JsonConvert.DeserializeObject<List<CState>>(objResponse.Data.ToString());
                            lstState = listState.Select(x => new SelectListItem { Text = x.State, Value = x.State }).ToList();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Common.errorlog("getAllState", Ex.Message.ToString(), Convert.ToString(Ex.InnerException));
            }

            return lstState;
        }

        public static bool SendEmail(string EmailTo, string EmailFrom, string EmailSubject, string EmailBody, string EmailCC = null)
        {
            bool Result = false;
            try
            {
                if (Convert.ToString(Convert.ToString(ConfigurationManager.AppSettings["IsMailSendFlag"])) == "true")
                {
                    MailMessage mail = new MailMessage();
                    //mail.From = new MailAddress(EmailFrom);
                    mail.From = new MailAddress(EmailFrom, ConfigurationManager.AppSettings["EmailDisplayName"]);
                    mail.To.Add(EmailTo);
                    if (!string.IsNullOrEmpty(EmailCC))
                    {
                        mail.CC.Add(EmailCC);
                    }
                    mail.Subject = EmailSubject;
                    mail.IsBodyHtml = true;
                    mail.Body = EmailBody;
                    SmtpClient Smtpobj = new SmtpClient(Convert.ToString(ConfigurationManager.AppSettings["Host"]));
                    Smtpobj.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                    Smtpobj.Credentials = new System.Net.NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["EmailUsername"]), Convert.ToString(ConfigurationManager.AppSettings["EmailPassword"]));
                    Smtpobj.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EmailSSL"]);
                    Smtpobj.Send(mail);
                    Result = true;
                }
            }
            catch (Exception ex)
            {
                Result = false;
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "SendEmail", "Common");
            }
            return Result;
        }

    }
}