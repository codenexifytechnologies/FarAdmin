using AssetsManagement_Admin.Areas.SuperAdmin.Data;
using AssetsManagement_Admin.Operations;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetsManagement_Admin.Areas.SuperAdmin.Controllers
{
    public class UploadController : Controller
    {
        // GET: SuperAdmin/Upload

        UploadOperations objUploadOperations = new UploadOperations();
        public ActionResult UploadAsset()
        {
            ViewBag.Company = new SelectList(Common.getAllCompany(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult UploadAsset(HttpPostedFileBase file, int CompanyId, string FARDate)
        {
            ViewBag.Company = new SelectList(Common.getAllCompany(), "Value", "Text");

            if (file == null || file.ContentLength == 0)
            {
                TempData["ErrorMessage"] = "Please select a CSV file.";
                return View();
            }

            UploadOperations uploadOperations = new UploadOperations();

            UploadAssetResult result = uploadOperations.UploadAsset(file, CompanyId, FARDate);

            if (!result.IsSuccess)
            {
                if (result.ErrorFileBytes != null)
                {
                    return File(result.ErrorFileBytes, "text/csv", result.ErrorFileName);
                }

                TempData["ErrorMessage"] = result.Message;
                return View();
            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction("UploadAsset");
        }
    }
}