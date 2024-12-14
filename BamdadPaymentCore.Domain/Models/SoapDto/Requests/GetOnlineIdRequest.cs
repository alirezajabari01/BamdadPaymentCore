using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Models.SoapDto.Requests
{
    public record GetOnlineIdRequest(string Username, string Password, string Price, string Desc, string ReqId, string Kind, bool AutoSettle, string OnlineType, string MobileNomber);
}



