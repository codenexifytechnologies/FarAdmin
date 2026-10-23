using AssetsManagement_Admin.Areas.SuperAdmin.Data;
using AssetsManagement_Admin.Models;
using AssetsManagement_Admin.Operations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AssetsManagement_Admin.Areas.SuperAdmin.Controllers
{
    public class DashboardController : Controller
    {
        DashboardOperations objDashboardOperations = new DashboardOperations();

        // GET: SuperAdmin/Dashboard
        public ActionResult Index()
        {
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

        // Live header counts; the copy stored in the session at login goes stale as records are added or deleted
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetDashboardCount()
        {
            DashboardCount objDashboardCount = objDashboardOperations.GetDashboardCount();

            User objUser = Session["User"] as User;
            if (objUser != null)
            {
                objUser.DashboardCount = objDashboardCount;
            }

            return Json(objDashboardCount, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(FilterRequest objFilterRequest)
        {

            ViewBag.Company = new SelectList(Common.getAllCompany(), "Value", "Text");
            ViewBag.VendorBranch = new SelectList(Common.getAllBranch(), "Value", "Text");
            ViewBag.Region = new SelectList(Common.getAllRegion(), "Value", "Text");
            ViewBag.State = new SelectList(Common.getAllState(), "Value", "Text");
            ViewBag.DashboardChart = objDashboardOperations.GetDashboardChart(objFilterRequest);
            return View();
        }


    }
}