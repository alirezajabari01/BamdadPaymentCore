using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.StoreProceduresModels.Parameters;
using Microsoft.AspNetCore.Mvc;
using PGTesterApp.Business;
using System.Collections.Specialized;
using System.Data;

namespace BamdadPaymentCore.Controllers
{
    public class SendToBank(IPaymentService paymentService, ISendToBankService sendToBankService) : Controller
    {
        public IActionResult Index() => Content(sendToBankService.SendToBank(Request.Query["OnlineID"]), "text/html");
    }
}
