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
        IOptions<PaymentGatewaySetting> paymentGatewaySetting,
        IMellatService mellatService) : ControllerBase
    {
        public IActionResult Index()
        {
            var t = paymentService.GetOnlineStatus(new GetOnlineStatusParameter("salamteacher", "mft", "241568"));
            return Ok();
        }
    }
}