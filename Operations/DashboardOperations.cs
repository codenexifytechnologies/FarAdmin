using AssetsManagement_Admin.Areas.SuperAdmin.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AssetsManagement_Admin.Operations
{
    public class DashboardOperations
    {
        string connectionString = ConfigurationManager.ConnectionStrings["con"].ToString();
        SqlConnection connection;
        public DashboardCount GetDashboardCount()
        {
            DashboardCount objDashboardCount = new DashboardCount();
            try
            {
                DataTable userDataTable = new DataTable();
                using (connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("usp_GetDashboardCount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            connection.Open();
                            adapter.Fill(userDataTable);
                        }
                    }
                }
                if (userDataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in userDataTable.Rows)
                    {
                        objDashboardCount = new DashboardCount()
                        {
                            Company = Convert.ToInt32(row["Company"].ToString()),
                            Vendor = Convert.ToInt32(row["Vendor"].ToString()),
                            Users = Convert.ToInt32(row["Users"].ToString()),
                            Branch = Convert.ToInt32(row["Branch"].ToString()),
                        };
                    }

                }
                else
                {
                    
                }

            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "GetDashboardCount", "DashboardOperations");
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return objDashboardCount;
        }

        public DashboardChart GetDashboardChart(FilterRequest objFilterRequest)
        {
            DashboardChart objDashboardChart = new DashboardChart();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompanyId", objFilterRequest.CompanyId);
                    parameters.Add("@Branch", objFilterRequest.BranchName);
                    parameters.Add("@Region", objFilterRequest.Region);
                    parameters.Add("@State", objFilterRequest.State);

                    objDashboardChart.objPieChartData = con.Query<PieChartData>("usp_GetPieChartData", parameters, commandType: CommandType.StoredProcedure).ToList();
                    objDashboardChart.objYBarChartData = con.Query<BarChartData>("usp_GetBarChartForPhysicalAvailableData", parameters, commandType: CommandType.StoredProcedure).ToList();
                    objDashboardChart.objNBarChartData = con.Query<BarChartData>("usp_GetBarChartForPhysicalNotAvailableData", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception Ex)
            {
                Common.errorlog("GetDashboardChartData", Ex.Message.ToString(), Convert.ToString(Ex.InnerException));
            }
            return objDashboardChart;
        }
    }
}