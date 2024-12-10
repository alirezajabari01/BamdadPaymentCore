using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Models.SoapDto.Requests
{
    public record GetOnlineIdRequest(string Username, string Password, string Price, string Desc, string ReqId);
    public class GetOnlineIdRequestMapper
    {
        public static GetOnlineIdRequest ToGetOnlineIdRequest(string username, string password, string price, string desc, string reqId)
           => new(username, password, price, desc, reqId);
    }
}



