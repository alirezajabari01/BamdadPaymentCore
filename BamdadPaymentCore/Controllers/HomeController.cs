using System.Diagnostics;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.SoapDto.Requests;
using BamdadPaymentCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BamdadPaymentCore.Controllers
{
    public class HomeController(IPaymentService paymentService,IHttpContextAccessor httpContextAccessor) : ControllerBase
    {
        

        public IActionResult Index()
        {
            string x = "0";
            if(!(x == "0"))
            {

            }
            if(x != "0")
            {

            }
            //var t = new AsanPardakhtProvider("","as",);

           //var t = paymentService.GetOnlineId(new Domain.SoapDto.Requests.GetOnlineIdRequest("asan", "mft", "100000", "sdf", "45"));
           //var t = paymentService.GetOnlineStatus(new GetOnlineStatusParameter("asan", "mft", "240914"));
           //var t = paymentService.ReqReversal(new Domain.SoapDto.Requests.ReqReversalRequest("asan", "mfyt", "508888888"));
          // var t = paymentService.ReqRefund(new ReqRefundRequest("asan", "mdft", "240914", "1000000"));
            return Ok();
        }

        
    }
}
