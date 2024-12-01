using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BamdadPaymentCore.Controllers
{
    public class ReturnFromBank(IReturnFromBankService returnFromBankService, IHttpContextAccessor httpContextAccessor, IPaymentService paymentService) : Controller
    {
        public IActionResult Index()
        {
            //TODO ViewBags
            if (Request.Method != HttpMethod.Post.ToString()) return View("fail");

            var res = returnFromBankService.ReturnUrlRedirectionFromBank(Request.QueryString.Value);

            return Redirect(res);
        }
    }
}
