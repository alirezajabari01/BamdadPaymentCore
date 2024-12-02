using BamdadPaymentCore.Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BamdadPaymentCore.Controllers
{
    public class ReturnBankWithAccept(IReturnBankWithAcceptService service) : Controller
    {
        public IActionResult Index()
        {
            ViewBag.SaleOrderId = Request.Query["SaleOrderId"];
            ViewBag.RefId = Request.Query["RefId"];
            ViewBag.SaleReferenceId = Request.Query["SaleReferenceId"];
            ViewBag.SaleReferenceId = Request.Query["CardHolderInfo"];
            ViewBag.ResCode = Request.Query["ResCode"];

            var res = "";
            if (Request.HasFormContentType)
            {
                res = service.RedirectToUrl(ViewBag.SaleOrderId, ViewBag.RefId, ViewBag.SaleReferenceId, ViewBag.SaleReferenceId, ViewBag.ResCode);
            }
            return Redirect(res);
        }
    }
}
