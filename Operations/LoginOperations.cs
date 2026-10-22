using AssetsManagement_Admin.Areas.SuperAdmin.Data;
using AssetsManagement_Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AssetsManagement_Admin.Operations
{
    public class LoginOperations
    {
        public User LoginAsync(LoginUser objLoginUser)
        {
            Response objResponse = new Response();
            User objUser = new User();
            using (var client = new HttpClient())
            {
                string input = JsonConvert.SerializeObject(objLoginUser);
                var content = new StringContent(input, Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["BaseAddress"]));
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res =  client.PostAsync("Login",content).Result;
                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    objResponse = JsonConvert.DeserializeObject<Response>(EmpResponse);
                    if (objResponse.StatusCode == 200 && objResponse.Data != null)
                    {
                        objUser = JsonConvert.DeserializeObject<User>(objResponse.Data.ToString()) ?? new User();
                        objLoginUser.Message = Convert.ToString(objResponse.Message);
                    }
                    else
                    {
                        objUser.SQL_Msg = "Invalid username or password";
                    }
                }
                else
                {
                    objUser.SQL_Msg = "Login service is not responding. Please try again.";
                }
            }
            return objUser;
        }
    }
}