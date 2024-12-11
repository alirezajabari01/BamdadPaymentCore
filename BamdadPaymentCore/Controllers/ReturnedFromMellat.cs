using BamdadPaymentCore.Domain.Exceptions;
using BamdadPaymentCore.Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BamdadPaymentCore.Controllers
{
    public class ReturnedFromMellat(IMellatService mellatService) : Controller
    {
        public IActionResult Index()
        {
            string result = mellatService.ProcessCallBackFromBank(Request);

            if (Uri.TryCreate(result, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                return Redirect(result);
            }

            throw new AppException(result);
        }
    }
}
