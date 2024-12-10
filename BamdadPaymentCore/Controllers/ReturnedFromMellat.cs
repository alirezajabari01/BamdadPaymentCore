using BamdadPaymentCore.Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BamdadPaymentCore.Controllers
{
    public class ReturnedFromMellat(IReturnFromBankService service) : Controller
    {
        public IActionResult Index()
        {
            string result = service.ReturnFromMellat(Request);
            return View();
        }
    }
}
