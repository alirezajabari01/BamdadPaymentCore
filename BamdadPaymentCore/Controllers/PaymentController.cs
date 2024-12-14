using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.Models.ControllerDto;
using BamdadPaymentCore.Domain.Models.SoapDto.Requests;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;
using Microsoft.AspNetCore.Mvc;

namespace BamdadPaymentCore.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class PaymentController(IPaymentService paymentService) : ControllerBase
    {
        [HttpPost]
        public ApiResult<string> GetOnlineId(GetOnlineIdRequest request) => paymentService.GetOnlineId(request);

        [HttpPost]
        public ApiResult<SelectOnlinePayResult> SettleAsanPardakht(ReqSettleOnlineRequest request) => paymentService.SettleAsan(request);

        [HttpPost]
        public ApiResult<SelectOnlinePayResult> SettleMellat(ReqSettleOnlineRequest request) => paymentService.SettleAsan(request);

        [HttpGet]
        public ApiResult<SelectOnlinePayResult> OnlineStatus([FromQuery] GetOnlineStatusParameter request) => paymentService.OnlineStatus(request);
    }
}