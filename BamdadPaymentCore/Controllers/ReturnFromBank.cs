using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BamdadPaymentCore.Controllers
{
    public class ReturnFromBank(IReturnFromBankService returnFromBankService) : Controller
    {
        //TODO if url nabood , show error 
        public IActionResult Index() => Redirect(returnFromBankService.ReturnUrlRedirectionFromBank(Request) +
                                                 "?OnlineID=" + Request.Query["invoiceid"]);
    }
}