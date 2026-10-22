using AssetsManagement_Admin.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetsManagement_Admin.Areas.SuperAdmin.Controllers
{
    public class DeleteController : Controller
    {
        DeleteOperations objDeleteOperations = new DeleteOperations();
        public ActionResult Delete()
        {
            ViewBag.Company = new SelectList(Common.getAllCompany(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int CompanyId)
        {
            string msg = objDeleteOperations.DeleteUserAssetData(CompanyId);
            TempData["SuccessMessage"] = msg;

            ViewBag.Company = new SelectList(Common.getAllCompany(), "Value", "Text");
            return View();
        }
    }
}