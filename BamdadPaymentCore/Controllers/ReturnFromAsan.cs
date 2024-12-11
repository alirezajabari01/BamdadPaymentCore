using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.Exceptions;
using BamdadPaymentCore.Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BamdadPaymentCore.Controllers
{
    public class ReturnFromAsan(IAsanRestService asanRestService, IPaymentService paymentService) : Controller
    {
        public IActionResult Index()
        {
            string onlineId = Request.Query["invoiceid"]!;
            string queryString = "?OnlineID=" + onlineId;
            
            if (string.IsNullOrEmpty(Request.Form["PaygateTranId"])) return Redirect(paymentService.CancelPayment(onlineId) + queryString);

            string result = asanRestService.ProcessAsanPardakhtPayment(onlineId) + queryString;

            if (Uri.TryCreate(result, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                return Redirect(result);
            }

            ViewBag.Message = result;
            return View();
        }
    }
}