using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.Exceptions;
using BamdadPaymentCore.Domain.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace BamdadPaymentCore.Controllers
{
    public class ReturnFromAsan(IAsanRestService asanRestService, IPaymentService paymentService) : Controller
    {
        public IActionResult Index()
        {
            string onlineId = Request.Query["invoiceid"]!;
            string queryString = "?OnlineID=" + onlineId;

            string result = asanRestService.ProcessAsanPardakhtPayment(onlineId) + queryString;

            if (Uri.TryCreate(result, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                return Redirect(result);
            }

            throw new ReturnFromAsanException();
        }
    }
}