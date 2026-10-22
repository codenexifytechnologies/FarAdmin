using AssetsManagement_Admin.Models;
using AssetsManagement_Admin.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AssetsManagement_Admin.Operations;
using AssetsManagement_Admin.Areas.SuperAdmin.Data;

namespace AssetsManagement_Admin.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        LoginOperations objLoginOperations = new LoginOperations();
        DashboardOperations objDashboardOperations = new DashboardOperations();
        public ActionResult Index()
        {
            return View();
        }

        #region User Login

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUser objLoginUser)
        {
            try
            {
                User objUser = objLoginOperations.LoginAsync(objLoginUser);
                if (objUser != null && string.IsNullOrEmpty(objUser.SQL_Msg) && !string.IsNullOrEmpty(objUser.Name))
                {
                    objUser.DashboardCount = objDashboardOperations.GetDashboardCount();
                    Session["User"] = objUser;
                    SessionAuthorizeAttribute.SignIn(Response, objUser);
                    return RedirectToAction("Index", "Dashboard", new { area = "SuperAdmin" });
                }
                TempData["ErrorMessage"] = objUser != null && !string.IsNullOrEmpty(objUser.SQL_Msg) ? objUser.SQL_Msg : "Invalid username or password";
            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message), "Login", "LoginController");
                TempData["ErrorMessage"] = "Unable to reach the server. Please try again.";
            }
            return View();
        }
        #endregion

        #region User Logout
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("login", "Login");
        }
        #endregion
    }
}