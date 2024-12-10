using BamdadPaymentCore.Domain.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;

namespace BamdadPaymentCore.Controllers
{
    public class SendToBank(IPaymentService paymentService, ISendToBankService sendToBankService, IJSRuntime jSRuntime) : Controller
    {
        public IActionResult Index()
        {
            var result = sendToBankService.SendToBank(Request.Query["OnlineID"]);

            if (string.IsNullOrEmpty(result.RefId))
            {
                if (!string.IsNullOrEmpty(result.Url))
                {
                    return Redirect(result.Url);
                }
                ViewBag.Message = result.Message;
                return View();
            }

            ViewBag.GatewayUrl = result.Url;
            ViewBag.RefId = result.RefId;
            return View();
        }
    }
}

