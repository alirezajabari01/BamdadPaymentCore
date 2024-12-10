using BamdadPaymentCore.Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BamdadPaymentCore.Controllers
{
    public class ReturnedFromMellat(IReturnFromBankService service) : Controller
    {
        public IActionResult Index()
        {
            string result = service.ReturnFromMellat(Request);

            if (Uri.TryCreate(result, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                return Redirect(result);
            }

            ViewBag.Message = result;
            return View();
        }
    }
}
