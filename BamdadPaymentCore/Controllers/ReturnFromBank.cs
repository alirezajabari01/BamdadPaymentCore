using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BamdadPaymentCore.Controllers
{
    public class ReturnFromBank(IReturnFromBankService returnFromBankService) : Controller
    {
        public IActionResult Index()
        {
            // Wrong is GET
            // Correct IS POST

            if (Request.Method == "GET") { ViewBag.SaleReferenceId = ""; ViewBag.PaymentStatusMessage = "enseraf"; return View(); }

            var ts = Request.Method;
            //var t = Request.Form["ReturningParams"];

            //ViewBag.SaleReferenceId = Request.Form["SaleReferenceId"];
            //ViewBag.RefId = Request.Form["RefId"];
            //ViewBag.SaleOrderId = Request.Form["SaleOrderId"];
            //ViewBag.ResCode = Request.Form["ResCode"];

            var res = returnFromBankService.ReturnUrlRedirectionFromBank(Request);
            //if (res != string.Empty)
            //{
            //    ViewBag.PaymentStatusMessage = "تراكنش با موفقيت انجام شد";
            //}
            //return Redirect(res);
            return View();  
        }
    }
}
