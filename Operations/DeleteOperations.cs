using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AssetsManagement_Admin.Operations
{
    public class DeleteOperations
    {
        string connectionString = ConfigurationManager.ConnectionStrings["con"].ToString();
        SqlConnection connection;
        public string DeleteUserAssetData(int CompanyId)
        {
            try
            {
                string Result = string.Empty;
                using (connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("usp_delete_asserdata_by_company", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CompanyId", CompanyId);
                        command.Connection.Open();
                        Result = (string)command.ExecuteScalar();
                    }
                }
                return Result;
            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "DeleteUser", "UserManagementOperations");
                return "User Action failed due to some technical issue, please try again later.";
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

    }
}