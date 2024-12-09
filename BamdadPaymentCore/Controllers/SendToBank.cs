using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.StoreProceduresModels.Parameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using PGTesterApp.Business;
using System.Collections.Specialized;
using System.Data;

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
                return View();
            }

            ViewBag.GatewayUrl = result.Url;
            ViewBag.RefId = result.RefId;
            return View();
        }
    }
}

