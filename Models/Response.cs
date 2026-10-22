using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetsManagement_Admin.Models
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}