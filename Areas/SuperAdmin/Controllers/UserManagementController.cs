using AssetsManagement_Admin.Areas.SuperAdmin.Data;
using AssetsManagement_Admin.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssetsManagement_Admin.Areas.SuperAdmin.Controllers
{
    public class UserManagementController : Controller
    {
        UserManagementOperations objUserManagementOperations = new UserManagementOperations();
        public ActionResult RegisterUser()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login", new { area = "" });
            }

            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(User objUser)
        {
            try
            {
                if (Session["User"] == null)
                {
                    return RedirectToAction("LogOff", "Login", new { area = "" });
                }

                //Common.SaveRequestResponse("RegisterUser", JsonConvert.SerializeObject(objUser),"");
                if (ModelState.IsValid)
                {
                    string strMsg = string.Empty;
                    strMsg = objUserManagementOperations.InsertUpdateClient(objUser, "I");

                    if (strMsg.Contains("Success"))
                    {
                        TempData["SuccessMessage"] = strMsg;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = strMsg;
                    }
                    ModelState.Clear();
                    return View(new User());
                }
                else
                {
                    ModelState.AddModelError("Error", "Please fiil all fileds");
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "RegisterUser", "UserManagement");
            }
            return View();

        }


        public ActionResult ManageUser()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login", new { area = "" });
            }

            List<User> lstUser = new List<User>();
            lstUser = objUserManagementOperations.GetUserRegistrationDetails();
            return View(lstUser);
        }

        public ActionResult EditManageUser(int Id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login", new { area = "" });
            }

            User objUser = new User();
            objUser = objUserManagementOperations.GetUserRegistrationDetailsByID(Id);
            return View(objUser);
        }
        [HttpPost]
        public ActionResult EditManageUser(User objUser)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login", new { area = "" });
            }

            if (ModelState.IsValid)
            {
                string strMsg = string.Empty;
                strMsg = objUserManagementOperations.InsertUpdateClient(objUser, "U");

                if (strMsg.Contains("Success"))
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
            return RedirectToAction("ManageUser", "UserManagement", new { area = "SuperAdmin" });
        }

        public ActionResult DeleteUser(int Id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("LogOff", "Login", new { area = "" });
            }

            string strMsg = string.Empty;
            strMsg = objUserManagementOperations.DeleteUser(Id);

            if (strMsg.Contains("Success"))
            {
                TempData["SuccessMessage"] = strMsg;
            }
            else
            {
                TempData["ErrorMessage"] = strMsg;
            }

            return RedirectToAction("ManageUser", "UserManagement", new { area = "SuperAdmin" });
        }
    }
}