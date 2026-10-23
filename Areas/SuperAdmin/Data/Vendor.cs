using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssetsManagement_Admin.Areas.SuperAdmin.Data
{
    public class Vendor
    {
        public int VendorId { get; set; }
        [Required(ErrorMessage = "Please enter the Name")]
        public string VendorName { get; set; }
        [Required(ErrorMessage = "Please enter the Contact No")]
        public string VendorContactNo { get; set; }
        [Required(ErrorMessage = "Please enter the Email Id")]
        public string VendorEmailId { get; set; }
        [Required(ErrorMessage = "Please select a Company")]
        public string VendorCompany { get; set; }
        [Required(ErrorMessage = "Please select a Branch")]
        public string VendorBranch { get; set; }
        public bool VendorVisibleReport { get; set; }
        public bool IsBothCameraOptionVisible { get; set; }
        public bool IsStaticLocationVisible { get; set; }
        [Required(ErrorMessage = "Please create a Password")]
        public string VendorPassword { get; set; }
        public int IsActive { get; set; }
        public string CreatedDate { get; set; }
        public string VendorMsg { get; set; }
    }
}