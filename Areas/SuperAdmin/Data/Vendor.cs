using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetsManagement_Admin.Areas.SuperAdmin.Data
{
    public class Vendor
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorContactNo { get; set; }
        public string VendorEmailId { get; set; }
        public string VendorCompany { get; set; }
        public string VendorBranch { get; set; }
        public bool VendorVisibleReport { get; set; }
        public bool IsBothCameraOptionVisible { get; set; }
        public bool IsStaticLocationVisible { get; set; }
        public string VendorPassword { get; set; }
        public int IsActive { get; set; }
        public string CreatedDate { get; set; }
        public string VendorMsg { get; set; }
    }
}