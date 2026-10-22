using System.Web;
using System.Web.Mvc;

namespace AssetsManagement_Admin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new SessionAuthorizeAttribute());
        }
    }
}
