using AssetsManagement_Admin.Areas.SuperAdmin.Data;
using AssetsManagement_Admin.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetsManagement_Admin.Areas.SuperAdmin.Controllers
{
    public class CompanyManagementController : Controller
    {
        // GET: SuperAdmin/CompanyManagement
        CompanyManagementOperations objCompanyManagementOperations = new CompanyManagementOperations();
        public ActionResult RegisterCompany()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login", new { area = "" });
            }
            return View();
        }

        [HttpPost]
        public ActionResult RegisterCompany(Company objCompany)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login", new { area = "" });
            }

            string SQL_Msg = string.Empty;
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                objCompany.CreatedBy = ((User)Session["User"]).Name;
                SQL_Msg = objCompanyManagementOperations.RegisterCompany(objCompany);

                if (SQL_Msg.Contains("successfully"))
                {
                    TempData["SuccessMessage"] = "Success! The company is registered Successfully.";
                    ModelState.Clear();
                    return View(new Company());
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

        public ActionResult CompanyDirectory()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login", new { area = "" });
            }

            List<Company> lstCompanyUser = new List<Company>();
            lstCompanyUser = objCompanyManagementOperations.CompanyDirectory();
            return View(lstCompanyUser);
        }

        public ActionResult EditCompanyDirectory(int Id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login", new { area = "" });
            }

            Company objCompany = new Company();
            objCompany = objCompanyManagementOperations.CompanyDirectoryById(Id);
            return View(objCompany);
        }

        [HttpPost]
        public ActionResult EditCompanyDirectory(Company objCompany)
        {
            string SQL_Msg = "";
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login", new { area = "" });
            }

            
            if (ModelState.IsValid)
            {
                SQL_Msg = objCompanyManagementOperations.EditRegisterCompany(objCompany);

                if (SQL_Msg.Contains("Success"))
                {
                    TempData["SuccessMessage"] = "Success! The company is modified successfully.";
                    ModelState.Clear();
                    return RedirectToAction("CompanyDirectory", "CompanyManagement", new { area = "SuperAdmin" }); ;
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

        public ActionResult DeleteCompanyDirectory(int Id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login", new { area = "" });
            }


            string SQL_Msg = string.Empty;
            SQL_Msg = objCompanyManagementOperations.DeleteCompany(Id);

            if (SQL_Msg.Contains("successfully"))
            {
                TempData["SuccessMessage"] = "Success! Company has been deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Oops! We couldn’t delete your Company. Please try again.";
            }

            return RedirectToAction("CompanyDirectory", "CompanyManagement", new { area = "SuperAdmin" });
        }

    }
}
