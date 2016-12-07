using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class ResponseModel
    {
        public int Status_Code { get; set; }

        public string Message { get; set; }
    }
}