using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models
{
    public interface IResponseVm
    {
        int ResCode { get; set; }
        string ResMessage { get; set; }
    }
}