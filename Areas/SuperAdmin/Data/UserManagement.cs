using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetsManagement_Admin.Areas.SuperAdmin.Data
{
    public class User
    {
        public int userId { get; set; }

        [Required(ErrorMessage = "The Name field is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The EmailId field is required")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "The Mobile No field is required")]
        public string mobileNo { get; set; }

        public string isAdmin { get; set; }

        public string Message { get; set; }

        public string Password { get; set; }

        public string IsActive { get; set; }

        public HttpPostedFileBase ProfileImage { get; set; }

        public string Image { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public string SQL_Msg { get; set; }

        public DashboardCount DashboardCount { get; set; }
    }

    public class DashboardCount
    {
        public int Company { get; set; }
        public int Vendor { get; set; }
        public int Users { get; set; }
        public int Branch { get; set; }
    }

}