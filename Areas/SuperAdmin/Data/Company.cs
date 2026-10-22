using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetsManagement_Admin.Areas.SuperAdmin.Data
{
    public class Company
    {
        public int CompanyId { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string CompanyContactPerson { get; set; }
        [Required]
        public string CompanyContactNumber { get; set; }
        [Required]
        public string CompanyContactEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isActive { get; set; }
        public string CompanyMsg { get; set; }


        public string CompanyContactPerson2 { get; set; }
        public string CompanyContactNumber2 { get; set; }
        public string CompanyContactEmail2 { get; set; }

        public string CompanyContactPerson3 { get; set; }
        public string CompanyContactNumber3 { get; set; }
        public string CompanyContactEmail3 { get; set; }
        public string CreatedBy { get; set; }

    }
}