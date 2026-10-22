using AssetsManagement_Admin.Areas.SuperAdmin.Data;
using AssetsManagement_Admin.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetsManagement_Admin.Areas.SuperAdmin.Controllers
{
    public class AssetController : Controller
    {
        AssetOperations objAssetOperations = new AssetOperations();
        public ActionResult AssetList()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login", new { area = "" });
            }
            ViewBag.Company = new SelectList(Common.getAllCompany(), "Value", "Text");
            ViewBag.VendorBranch = new SelectList(Common.getAllBranch(), "Value", "Text");

            //List<AssetData> lstAssetData = new List<AssetData>();
            //lstAssetData = objAssetOperations.GetAssetList();
            return View();
        }

        [HttpPost]
        public ActionResult AssetList(FilterRequest objFilterRequest)
        {

            ViewBag.Company = new SelectList(Common.getAllCompany(), "Value", "Text");
            ViewBag.VendorBranch = new SelectList(Common.getAllBranch(), "Value", "Text");
            ViewBag.lstAssetData = objAssetOperations.GetAssetList(objFilterRequest);
            return View();
        }

        [HttpGet]
        public ActionResult AssetListById(int Id)
        {
            AssetData objAssetData = objAssetOperations.GetAssetListById(Id);
            return View(objAssetData);
        }

        [HttpPost]
        public ActionResult UpdateAsset(AssetData objAssetData)
        {
            ViewBag.lstAssetData = objAssetOperations.UpdateAsset(objAssetData);
            return View();
        }
    }
}