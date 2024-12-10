using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models
{


    public interface ITokenVm : IResponseVm
    {
        string RefId { get; set; }

    }

}