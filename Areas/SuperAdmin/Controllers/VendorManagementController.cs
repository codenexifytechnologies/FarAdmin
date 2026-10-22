using AssetsManagement_Admin.Areas.SuperAdmin.Data;
using AssetsManagement_Admin.Operations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace AssetsManagement_Admin.Areas.SuperAdmin.Controllers
{
    public class VendorManagementController : Controller
    {
        // GET: SuperAdmin/VendorManagement
        VendorManagementOperations objVendorManagementOperations = new VendorManagementOperations();
        public ActionResult RegisterVendor()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login", new { area = "" });
            }
            ViewBag.Company = new SelectList(Common.getAllCompany(), "Value", "Text");
            ViewBag.VendorBranch = new SelectList(Common.getAllBranch(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult RegisterVendor(Vendor objVendor)
        {
            try
            {
                if (Session["User"] == null)
                {
                    return RedirectToAction("LogOff", "Login/Login", new { area = "" });
                }
                ViewBag.Company = new SelectList(Common.getAllCompany(), "Value", "Text");
                ViewBag.VendorBranch = new SelectList(Common.getAllBranch(), "Value", "Text");

                string SQL_Msg = string.Empty;

                if (ModelState.IsValid)
                {
                    SQL_Msg = objVendorManagementOperations.RegisterVendor(objVendor);

                    if (SQL_Msg.Contains("successfully"))
                    {
                        TempData["SuccessMessage"] = "Success! The Vendor is registered Successfully.";
                        ModelState.Clear();
                        return View(new Vendor());
                    }
                    else
                    {
                        TempData["ErrorMessage"] = SQL_Msg;
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                }
                return View();
            }

            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "RegisterVendor", "VendorManagementController");
                TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again later.";
                return View();
            }
        }
        public ActionResult ManageVendor()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login", new { area = "" });
            }

            List<Vendor> lstVendor = new List<Vendor>();
            lstVendor = objVendorManagementOperations.VendorDirectory();
            return View(lstVendor);
        }

        public ActionResult EditManageVendor(int Id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login/Login", new { area = "" });
            }

            Vendor objVendor = new Vendor();
            objVendor = objVendorManagementOperations.VendorDirectoryById(Id);
            ViewBag.Company = new SelectList(Common.getAllCompany(), "Value", "Text");
            return View(objVendor);
        }
        [HttpPost]
        public ActionResult EditManageVendor(Vendor objVendor)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login/Login", new { area = "" });
            }

            if (ModelState.IsValid)
            {
                string strMsg = string.Empty;
                strMsg = objVendorManagementOperations.EditRegisterVendor(objVendor);

                if (strMsg.Contains("Successfully"))
                {
                    TempData["SuccessMessage"] = strMsg;
                }
                else
                {
                    TempData["ErrorMessage"] = strMsg;
                }
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("Error", "Please fiil all fileds");
            }
            return RedirectToAction("EditManageVendor", "VendorManagement", new { area = "SuperAdmin" , Id= objVendor.VendorId });
        }

        public ActionResult DeleteVendor(int Id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login", new { area = "" });
            }

            string strMsg = string.Empty;
            strMsg = objVendorManagementOperations.DeleteVendor(Id);

            if (strMsg.Contains("Success"))
            {
                TempData["SuccessMessage"] = strMsg;
            }
            else
            {
                TempData["ErrorMessage"] = strMsg;
            }

            return RedirectToAction("ManageVendor", "DeleteVendor", new { area = "SuperAdmin" });
        }


        [HttpGet]
        public JsonResult getBranch(int CompanyID)
        {
            var result = Common.getFIlterData(CompanyID, null, null, null);

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

    }
}