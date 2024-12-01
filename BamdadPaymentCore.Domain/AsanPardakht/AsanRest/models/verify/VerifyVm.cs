using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.models.verify
{
    public class VerifyVm : IResponseVm
    {
        public int ResCode { get; set; }
        public string ResMessage { get; set; }
    }

}