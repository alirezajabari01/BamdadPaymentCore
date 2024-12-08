using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.Exceptions;
using BamdadPaymentCore.Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BamdadPaymentCore.Controllers
{
    public class ReturnFromBank(IReturnFromBankService returnFromBankService) : Controller
    {
        public IActionResult Index()
        {
            string result = returnFromBankService.ReturnUrlRedirectionFromBank(Request) + "?OnlineID=" + Request.Query["invoiceid"];

            if (Uri.TryCreate(result, UriKind.Absolute, out var uriResult) &&
                (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                return Redirect(result);
            }

            return View();
        }
    }
}