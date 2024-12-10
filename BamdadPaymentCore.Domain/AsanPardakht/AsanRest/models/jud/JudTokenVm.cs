using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.jud
{
    public class JudTokenVm : ITokenVm
    {
        public string RefId { get; set; }
        public int ResCode { get; set; }
        public string ResMessage { get; set; }
    }
}