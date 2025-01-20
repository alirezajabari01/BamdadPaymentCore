using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.Models.ServicesModels;
using BamdadPaymentCore.Domain.Models.SoapDto.Requests;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BamdadPaymentCore.Controllers
{
    public class HomeController(
        IPaymentService paymentService,
        IHttpContextAccessor httpContextAccessor,
        IBamdadPaymentRepository bamdadPaymentRepository,
        IOptionsSnapshot<PaymentGatewaySetting> paymentGatewaySetting,
        IMellatService mellatService) : ControllerBase
    {
        public IActionResult Index()
        {
            paymentService.CancelPayment("241584");
            return Ok();
        }
    }
}