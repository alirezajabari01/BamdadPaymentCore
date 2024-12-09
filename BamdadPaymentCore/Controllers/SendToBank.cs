using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.StoreProceduresModels.Parameters;
using Microsoft.AspNetCore.Mvc;
using PGTesterApp.Business;
using System.Collections.Specialized;
using System.Data;

namespace BamdadPaymentCore.Controllers
{
    public class SendToBank(IPaymentService paymentService, ISendToBankService sendToBankService) : Controller
    {
        public IActionResult Index()
        {
            var result = sendToBankService.SendToBank(Request.Query["OnlineID"]);

            if (!string.IsNullOrEmpty(result))
            {
                if (Uri.TryCreate(result, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                {
                    return Redirect(result);
                }

                if (result.Contains("<script", StringComparison.OrdinalIgnoreCase) || result.Contains("<html", StringComparison.OrdinalIgnoreCase))
                {
                    return Content(result, "text/html");
                }
            }

            return View();
        }

    }
}

