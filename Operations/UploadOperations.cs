using AssetsManagement_Admin.Areas.SuperAdmin.Data;
using AssetsManagement_Admin.Models;
using CsvHelper;
using CsvHelper.Configuration;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;



namespace AssetsManagement_Admin.Operations
{
    public class UploadOperations
    {
        public UploadAssetResult UploadAsset(HttpPostedFileBase file, int CompanyId, string FARDate)
        {
            try
            {
                DateTime dtFarDate = Convert.ToDateTime(FARDate);

                List<AssetData> finalList = new List<AssetData>();
                List<ValidationError> validationErrors = new List<ValidationError>();

                string[] headers = null;

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    MissingFieldFound = null,
                    HeaderValidated = null,
                    BadDataFound = null
                };

                using (var reader = new StreamReader(file.InputStream))
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Read();
                    csv.ReadHeader();

                    headers = csv.HeaderRecord;

                    int rowNo = 2;

                    while (csv.Read())
                    {
                        List<string> errors = ValidateRow(csv);

                        if (errors.Any())
                        {
                            validationErrors.Add(new ValidationError
                            {
                                RowNo = rowNo,
                                ErrorMessage = string.Join(" | ", errors),
                                RowData = csv.Parser.Record
                            });

                            rowNo++;
                            continue;
                        }

                        int qty = Convert.ToInt32(csv.GetField(12));

                        decimal currentAPC = Convert.ToDecimal(csv.GetField(5));

                        decimal currBkVal = Convert.ToDecimal(csv.GetField(6));

                        DateTime capDate = Convert.ToDateTime(csv.GetField(4));

                        int days = (dtFarDate - capDate).Days;

                        if (qty > 1)
                        {
                            decimal apcPerItem = currentAPC / qty;
                            decimal bkValPerItem = currBkVal / qty;

                            for (int i = 1; i <= qty; i++)
                            {
                                finalList.Add(CreateAsset(csv, CompanyId, FARDate, days, apcPerItem.ToString(), bkValPerItem.ToString(), i));
                            }
                        }
                        else
                        {
                            finalList.Add(CreateAsset(csv, CompanyId, FARDate, days, csv.GetField(5), csv.GetField(6), 0));
                        }

                        rowNo++;
                    }
                }

                if (validationErrors.Any())
                {
                    return new UploadAssetResult
                    {
                        IsSuccess = false,
                        Message = "Validation errors found.",
                        ErrorFileBytes = GenerateErrorFile(validationErrors, headers),
                        ErrorFileName = "AssetUploadErrors.csv"
                    };
                }

                Response objResponse = new Response();
                using (var client = new HttpClient())
                {
                    // Large FAR uploads take longer than HttpClient's default 100 seconds
                    client.Timeout = TimeSpan.FromMinutes(10);
                    string input = JsonConvert.SerializeObject(finalList);
                    var content = new StringContent(input, Encoding.UTF8, "application/json");
                    client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage Res = client.PostAsync("UploadAssetData", content).Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                        objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);
                        if (objResponse.StatusCode == 200)
                        {
                            return new UploadAssetResult
                            {
                                IsSuccess = true,
                                Message = objResponse.Data.ToString()
                            };
                        }
                        else
                        {
                            return new UploadAssetResult
                            {
                                IsSuccess = true,
                                Message = objResponse.Message.ToString()
                            };
                        }
                    }
                    else
                    {
                        return new UploadAssetResult
                        {
                            IsSuccess = false,
                            Message = "Failed to upload asset data. Please try again."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Common.errorlog(
                    ex.Message + " Inner Msg:" + ex.StackTrace,
                    "UploadAsset",
                    "UploadOperations");

                return new UploadAssetResult
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        private AssetData CreateAsset(CsvReader csv, int companyId, string farDate, int ageing, string apc, string bkVal, int itemNo)
        {
            return new AssetData
            {
                ASSET_NUMBER = itemNo > 0
                    ? csv.GetField(0) + "-" + itemNo
                    : csv.GetField(0),

                ASSET_CLASS = csv.GetField(1),
                BAL_SH_ACCT_APC = csv.GetField(2),
                ASSET_DESCRIPTION = csv.GetField(3),
                CAPITALIZED_ON = csv.GetField(4),
                CURRENT_APC = apc,
                CURR_BK_VAL = bkVal,
                DEACTIVATION_ON = csv.GetField(7),
                DATE_OF_ASSET_SOLD = csv.GetField(8),
                LOCATION = csv.GetField(9),
                LOCATION_DESCRIPTION = csv.GetField(10),
                COST_CENTER = csv.GetField(11),
                QUANTITIES = itemNo > 0 ? "1" : csv.GetField(12),
                DESCRIPTION_KEY = csv.GetField(13),
                RATE_OF_DEP = csv.GetField(14),
                SERIAL_NUMBER = csv.GetField(15),
                INVENTORY_NO = csv.GetField(16),
                ASSET_TAG = csv.GetField(17),
                DOCUMENT_NUMBER = csv.GetField(18),
                DOCUMENT_DATE = csv.GetField(19),
                AUC_DOCUMENT = csv.GetField(20),
                AUC_DOCUMENT_DATE = csv.GetField(21),
                AUC_DOCUMENT_TAX_C = csv.GetField(22),
                VENDOR_CODE = csv.GetField(23),
                NAME_OF_VENDOR = csv.GetField(24),
                BILL_NO = csv.GetField(25),
                TYPE = csv.GetField(26).ToUpper(),
                SCOPE = csv.GetField(27),
                TYPE_URCFO = csv.GetField(28).ToUpper(),
                SUBTYPE_URCFO = csv.GetField(29).ToUpper(),
                Region = csv.GetField(30).ToUpper(),
                State = csv.GetField(31).ToUpper(),
                CompanyId = companyId.ToString(),
                AGEING = ageing,
                NUM_PHOTO = Convert.ToInt32(csv.GetField(33)),
                FARDate = farDate
            };
        }

        private List<string> ValidateRow(CsvReader csv)
        {
            List<string> errors = new List<string>();

            if (!decimal.TryParse(csv.GetField(2), out decimal APC))
                errors.Add("BAL SH ACCT APC must be decimal");

            if (string.IsNullOrWhiteSpace(csv.GetField(3)))
                errors.Add("ASSET DESCRIPTION is mandatory");

            string capitalizedOn = csv.GetField(4);

            if (string.IsNullOrWhiteSpace(capitalizedOn))
            {
                errors.Add("CAPITALIZED ON is mandatory");
            }
            else
            {
                if (!IsValidDate(capitalizedOn))
                {
                    errors.Add("CAPITALIZED ON must be in dd-MMM-yy format (e.g. 01-Jan-2025)");
                }
            }

            if (!decimal.TryParse(csv.GetField(5), out decimal CAPC))
                errors.Add("CURRENT APC is mandatory");

            if (!decimal.TryParse(csv.GetField(6), out decimal CBAPC))
                errors.Add("CURR BK VAL is mandatory");

            string DEACTIVATIONON = csv.GetField(7);

            if (string.IsNullOrWhiteSpace(DEACTIVATIONON))
            {
                errors.Add("DEACTIVATI ON is mandatory");
            }
            else
            {
                if (!IsValidDate(DEACTIVATIONON))
                {
                    errors.Add("DEACTIVATION ON must be in dd-MMM-yy format (e.g. 01-Jan-2025)");
                }
            }

            string DATEOFASSETSOLD = csv.GetField(8);

            if (string.IsNullOrWhiteSpace(DATEOFASSETSOLD))
            {
                errors.Add("DATE OF ASSET SOLD is mandatory");
            }
            else
            {
                if (!IsValidDate(DATEOFASSETSOLD))
                {
                    errors.Add("DATE OF ASSET SOLD must be in dd-MMM-yy format (e.g. 01-Jan-2025)");
                }
            }

            if (string.IsNullOrWhiteSpace(csv.GetField(9)))
                errors.Add("LOCATION is mandatory");

            if (string.IsNullOrWhiteSpace(csv.GetField(10)))
                errors.Add("LOCATION DESCRIPTION is mandatory");

            if (!decimal.TryParse(csv.GetField(11), out decimal COSTCENTER))
                errors.Add("COST CENTER is mandatory");

            if (!int.TryParse(csv.GetField(12), out int qty))
                errors.Add("QUANTITIES must be numeric");
            else if (qty <= 0)
                errors.Add("QUANTITIES must be greater than 0");

            if (!decimal.TryParse(csv.GetField(14), out decimal RATEOFDEP))
                errors.Add("RATE OF DEP is mandatory");

            if (string.IsNullOrWhiteSpace(csv.GetField(17)))
                errors.Add("ASSET TAG is mandatory");

            string DOCUMENTDATE = csv.GetField(19);

            if (string.IsNullOrWhiteSpace(DOCUMENTDATE))
            {
                errors.Add("DEACTIVATI ON is mandatory");
            }
            else
            {
                if (!IsValidDate(DOCUMENTDATE))
                {
                    errors.Add("DOCUMENT DATE must be in dd-MMM-yy format (e.g. 01-Jan-2025)");
                }
            }

            string AUCDOCUMENTDATE = csv.GetField(21);

            if (string.IsNullOrWhiteSpace(AUCDOCUMENTDATE))
            {
                errors.Add("AUC DOCUMENT DATE is mandatory");
            }
            else
            {
                if (!IsValidDate(AUCDOCUMENTDATE))
                {
                    errors.Add("AUC DOCUMENT DATE must be in dd-MMM-yy format (e.g. 01-Jan-2025)");
                }
            }

            if (string.IsNullOrWhiteSpace(csv.GetField(26)))
                errors.Add("TYPE is mandatory");

            if (string.IsNullOrWhiteSpace(csv.GetField(27)))
                errors.Add("SCOPE is mandatory");

            if (string.IsNullOrWhiteSpace(csv.GetField(28)))
                errors.Add("TYPE URCFO is mandatory");

            if (string.IsNullOrWhiteSpace(csv.GetField(29)))
                errors.Add("SUBTYPE URCFO is mandatory");

            if (string.IsNullOrWhiteSpace(csv.GetField(30)))
                errors.Add("REGION is mandatory");

            if (string.IsNullOrWhiteSpace(csv.GetField(31)))
                errors.Add("STATE is mandatory");

            if (!int.TryParse(csv.GetField(32), out int AGEING))
                errors.Add("AGEING must be numeric");

            if (string.IsNullOrWhiteSpace(csv.GetField(33)))
                errors.Add("NUM_PHOTO is mandatory");

            if (!int.TryParse(csv.GetField(33), out int NUM_PHOTO))
                errors.Add("NUM_PHOTO must be numeric");

            return errors;
        }

        private byte[] GenerateErrorFile(List<ValidationError> errors, string[] headers)
        {
            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(ms))
            using (CsvWriter csv = new CsvWriter(sw, CultureInfo.InvariantCulture))
            {
                foreach (var header in headers)
                    csv.WriteField(header);

                csv.WriteField("ERROR MESSAGE");
                csv.NextRecord();

                foreach (var row in errors)
                {
                    foreach (var field in row.RowData)
                        csv.WriteField(field);

                    csv.WriteField(row.ErrorMessage);
                    csv.NextRecord();
                }

                sw.Flush();

                return ms.ToArray();
            }
        }

        public bool IsValidDate(string date)
        {
            DateTime dt;
            if (DateTime.TryParseExact(
                    date,
                    "dd-MMM-yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out dt))
            {
                return true;
            }
            return false;
        }
    }
}