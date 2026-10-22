using AssetsManagement_Admin.Areas.SuperAdmin.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace AssetsManagement_Admin.Operations
{
    public class UserManagementOperations
    {
        string connectionString = ConfigurationManager.ConnectionStrings["con"].ToString();
        SqlConnection connection;
        public string InsertUpdateClient(User objUser, string Action)
        {
            try
            {
                objUser.Password = ConfigurationManager.AppSettings["UserDefaultPass"].ToString();
                string path = "";
                string Result = string.Empty;
                if (objUser.ProfileImage != null && objUser.ProfileImage.ContentLength > 0)
                {
                    var filename = Path.GetFileName(objUser.ProfileImage.FileName);
                    path = Path.Combine(HttpContext.Current.Server.MapPath("~/Areas/SuperAdmin/Uploads/"), filename);

                    objUser.ProfileImage.SaveAs(path);
                }
                else if (objUser.Image != null && !string.IsNullOrEmpty(objUser.Image))
                {
                    path = objUser.Image;
                }
                else
                {
                    path = "assets/images/faces/6.jpg";
                }

                using (connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("usp_InsertUserRegistration", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ACTION", Action);
                        command.Parameters.AddWithValue("@UserId", objUser.userId);
                        command.Parameters.AddWithValue("@NAME", objUser.Name);
                        command.Parameters.AddWithValue("@EMAILID", objUser.EmailId);
                        command.Parameters.AddWithValue("@PASSWORD", objUser.Password);
                        command.Parameters.AddWithValue("@MOBILE", objUser.mobileNo);
                        command.Parameters.AddWithValue("@IMAGEPATH", path);

                        command.Connection.Open();
                        Result = (string)command.ExecuteScalar();
                    }
                }

                if (Result == "Success! The user has been added successfully." && Action == "I")
                {
                    string subject = Convert.ToString(ConfigurationManager.AppSettings["User_Registration"]);
                    subject = subject.Replace("{{user_name}}", objUser.Name);
                    string body = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Emails/User_Registration.html"));
                    body = body.Replace("{{user_name}}", objUser.Name);
                    body = body.Replace("{{user_id}}", objUser.EmailId);
                    body = body.Replace("{{password}}", objUser.Password);
                    body = body.Replace("{{login_link}}", Convert.ToString(ConfigurationManager.AppSettings["LoginUrl"]));
                    Common.SendEmail(objUser.EmailId, Convert.ToString(ConfigurationManager.AppSettings["FromEmail"]), subject, body);
                }
                else if (Result == "Success! The user has been updated successfully." && Action == "U")
                {
                    string subject = Convert.ToString(ConfigurationManager.AppSettings["User_Detail_Updation"]);
                    subject = subject.Replace("{{user_name}}", objUser.Name);
                    string body = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Emails/User_Detail_Updation.html"));
                    body = body.Replace("{{user_name}}", objUser.Name);
                    body = body.Replace("{{user_id}}", objUser.EmailId);
                    body = body.Replace("{{password}}", objUser.Password);
                    body = body.Replace("{{login_link}}", Convert.ToString(ConfigurationManager.AppSettings["LoginUrl"]));
                    Common.SendEmail(objUser.EmailId, Convert.ToString(ConfigurationManager.AppSettings["FromEmail"]), subject, body);
                }

                return Result;
            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "InsertUpdateClient", "UserManagementOperations");
                return "User creation failed due to some technical issue, please try again later.";
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public List<User> GetUserRegistrationDetails()
        {
            List<User> lstUser = new List<User>();
            User objUser = new User();
            try
            {
                DataTable userDataTable = new DataTable();
                using (connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("usp_GetUserRegistrationDetails", connection))
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
                        objUser = new User()
                        {
                            userId = Convert.ToInt32(row["userId"].ToString()),
                            Name = row["name"].ToString(),
                            EmailId = row["EmailId"].ToString(),
                            mobileNo = row["mobileNo"].ToString(),
                            IsActive = row["isActive"].ToString() == "1" ? "Active" : "InActive",
                        };
                        lstUser.Add(objUser);
                    }

                }
                else
                {
                    objUser.Message = "Error while data fetching..";
                }

            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "GetUserRegistrationDetails", "UserManagementOperations");

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return lstUser;
        }

        public User GetUserRegistrationDetailsByID(int UserId)
        {
            List<User> lstUser = new List<User>();
            User objUser = new User();
            try
            {
                DataTable userDataTable = new DataTable();
                using (connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("GetUserRegistrationDetailsByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", UserId);
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
                        objUser = new User()
                        {
                            userId = Convert.ToInt32(row["userId"].ToString()),
                            Name = row["name"].ToString(),
                            EmailId = row["EmailId"].ToString(),
                            Password = row["Password"].ToString(),
                            mobileNo = row["mobileNo"].ToString(),
                            IsActive = row["isActive"].ToString() == "1" ? "Active" : "InActive",
                            Image = row["ImagePath"].ToString(),
                        };
                        //lstUser.Add(objUser);
                    }

                }
                else
                {
                    objUser.Message = "Login Failed";
                }

            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "GetUserRegistrationDetailsByID", "UserManagementOperations");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return objUser;
        }

        public string UserActionRegistartion(int Id, string Action)
        {
            try
            {
                string Result = string.Empty;
                using (connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("usp_Operation_User", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Action", Action);
                        command.Parameters.AddWithValue("@UserId", Id);
                        command.Connection.Open();
                        Result = (string)command.ExecuteScalar();
                    }
                }
                return Result;
            }
            catch (Exception ex)
            {
                Common.errorlog(Convert.ToString(ex.Message) + " Inner Msg:" + Convert.ToString(ex.StackTrace), "UserActionRegistartion", "UserManagementOperations");
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

        public string DeleteUser(int Id)
        {
            try
            {
                string Result = string.Empty;
                using (connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("usp_DeleteUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", Id);
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