using AssetsManagement_Admin.Areas.SuperAdmin.Data;
using AssetsManagement_Admin.Operations;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AssetsManagement_Admin
{
    /// <summary>
    /// Ensures Session["User"] is present for every non-anonymous action.
    /// InProc session is lost on app pool recycle / idle shutdown, so the user is
    /// restored from the forms authentication cookie instead of being logged out.
    /// </summary>
    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }

            var httpContext = filterContext.HttpContext;
            if (httpContext.Session["User"] != null)
            {
                return;
            }

            User objUser = RestoreUserFromTicket(httpContext);
            if (objUser != null)
            {
                httpContext.Session["User"] = objUser;
                return;
            }

            if (httpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
                filterContext.Result = new HttpStatusCodeResult(401, "Session expired");
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(
                    new { controller = "Login", action = "LogOff", area = "" }));
            }
        }

        public static void SignIn(HttpResponseBase response, User objUser)
        {
            var ticketUser = new User
            {
                userId = objUser.userId,
                Name = objUser.Name,
                EmailId = objUser.EmailId,
                mobileNo = objUser.mobileNo,
                isAdmin = objUser.isAdmin,
                RoleID = objUser.RoleID,
                RoleName = objUser.RoleName
            };

            var ticket = new FormsAuthenticationTicket(1, objUser.EmailId ?? objUser.Name ?? "user", DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout), false, JsonConvert.SerializeObject(ticketUser));

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath
            };
            response.Cookies.Add(cookie);
        }

        private static User RestoreUserFromTicket(HttpContextBase httpContext)
        {
            try
            {
                HttpCookie cookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                {
                    return null;
                }

                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket == null || ticket.Expired || string.IsNullOrEmpty(ticket.UserData))
                {
                    return null;
                }

                User objUser = JsonConvert.DeserializeObject<User>(ticket.UserData);
                objUser.DashboardCount = new DashboardOperations().GetDashboardCount();
                return objUser;
            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message), "RestoreUserFromTicket", "SessionAuthorizeAttribute");
                return null;
            }
        }
    }
}
