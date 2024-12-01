using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.models
{


    public interface ITokenVm : IResponseVm
    {
        string RefId { get; set; }

    }

}