using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Web;

namespace AssetsManagement_Admin.Areas.SuperAdmin.Data
{
    public class AssetData
    {
        public int Id { get; set; }
        public string ASSET_NUMBER { get; set; }
        public string ASSET_CLASS { get; set; }
        public string BAL_SH_ACCT_APC { get; set; }
        public string ASSET_DESCRIPTION { get; set; }
        public string CAPITALIZED_ON { get; set; }
        public string CURRENT_APC { get; set; }
        public string CURR_BK_VAL { get; set; }
        public string DEACTIVATION_ON { get; set; }
        public string DATE_OF_ASSET_SOLD { get; set; }
        public string LOCATION { get; set; }
        public string LOCATION_DESCRIPTION { get; set; }
        public string COST_CENTER { get; set; }
        public string QUANTITIES { get; set; }
        public string DESCRIPTION_KEY { get; set; }
        public string RATE_OF_DEP { get; set; }
        public string SERIAL_NUMBER { get; set; }
        public string INVENTORY_NO { get; set; }
        public string ASSET_TAG { get; set; }
        public string DOCUMENT_NUMBER { get; set; }
        public string DOCUMENT_DATE { get; set; }
        public string AUC_DOCUMENT { get; set; }
        public string AUC_DOCUMENT_DATE { get; set; }
        public string AUC_DOCUMENT_TAX_C { get; set; }
        public string VENDOR_CODE { get; set; }
        public string NAME_OF_VENDOR { get; set; }
        public string BILL_NO { get; set; }
        public string TYPE { get; set; }
        public string SCOPE { get; set; }
        public string TYPE_URCFO { get; set; }
        public string SUBTYPE_URCFO { get; set; }
        public string CompanyId { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public int IsVerified { get; set; }
        public int IsPhysicalAVailable { get; set; }
        public string BarcodeNo { get; set; }
        public string AssetCondition { get; set; }
        public string AssetPhoto1 { get; set; }
        public string AssetPhoto2 { get; set; }
        public string Remark { get; set; }
        public string DeviceName { get; set; }
        public string Model { get; set; }
        public string GeoLocation { get; set; }
        public int VendorId { get; set; }
        public int IsAssetFound { get; set; }
        public string ITUserId { get; set; }
        public string Region { get; set; }
        public string State { get; set; }
        public int AGEING { get; set; }
        public string Floor { get; set; }
        public string Wing { get; set; }
        public string FARDate { get; set; }
        public int NUM_PHOTO { get; set; }
        public bool IsFirstCycleUpdate { get; set; }
    }

    public class ValidationError
    {
        public int RowNo { get; set; }
        public string ErrorMessage { get; set; }
        public string[] RowData { get; set; }
    }

    public class UploadAssetResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public byte[] ErrorFileBytes { get; set; }
        public string ErrorFileName { get; set; }
    }
}