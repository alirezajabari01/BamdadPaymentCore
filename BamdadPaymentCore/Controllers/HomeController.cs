using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.Models.ServicesModels;
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
            //SelectPaymentDetailResult paymentDetail = bamdadPaymentRepository.SelectPaymentDetail(new SelectPaymentDetailParameter("3513131"));
            // string card = "72B33C99E800FCCC677F1FC6AE13AAD11476A1E4E1565CF759";
            //var e = mellatService.SettleTransaction(new MellatRequest(4959335, "nimkat1398", "47637143", 241461, 241461, 292875991676));
            //var t = paymentService.ReqRefund(new ReqRefundRequest("asan", "mft", "241437", "534"));
            // var t =paymentService.ReqSettleOnline(new ReqSettleOnlineRequest("asan", "mft", 241419.ToString()));
            //var updateResult = bamdadPaymentRepository.UpdateOnlinePayment(new UpdateOnlinePayParameter("060241209938",
            //  241415, "e29cc467bc48c25a9", "2055610183", 0, "610433xxxx6724"));

            //var tt= paymentService.Settle("241415");
            //var t = bamdadPaymentRepository.SelectSiteAuthentication(
            //    new SiteAuthenticationParameter("asan", "mft", "::1"));
            //
            // var ts = bamdadPaymentRepository.SelectSiteAuthentication(
            //     new SiteAuthenticationParameter("asan", "mfts", "::1"));
            //var t = asanResetService.TransactionResult(Convert.ToInt32(paymentGatewaySetting.Value.AsanMerchantId), 241310,
            //     bamdadPaymentRepository.SelectPaymentDetail(new Domain.StoreProceduresModels.Parameters.SelectPaymentDetailParameter
            //     (241310.ToString()))).Result;
            // bamdadPaymentRepository.UpdateOnlinePayment(
            //     new Domain.StoreProceduresModels.Parameters.UpdateOnlinePayParameter(t.Rrn, 241310, t.RefId, t.PayGateTranID.ToString(), 0, t.CardNumber));
            //var t = paymentService.SelectPaymentDetail(new Domain.StoreProceduresModels.Parameters.SelectPaymentDetailParameter("241297"));
            //var p = asanResetService.TransactionResult(262755, 241297, t).Result;
            //var t = paymentService.CancelPayment("241282");

            //var t = new AsanPardakhtProvider("","as",);
            //var t = paymentService.UpdateOnlinePayFailed("241241", "cancel", "", "-1", "use cancel payment");
            var t = paymentService.GetOnlineIdDifferentTypes("asan", "mft", "100000", "sdf", "45", "0", mobleNomber: "09039423499");
            //var t = paymentService.GetOnlineIdDifferentTypes("asan", "mft", "100000", "sdf", "45","0",true);
            //var t = paymentService.GetOnlineStatus(new GetOnlineStatusParameter("asan", "mft", "240914"));
            //var t = paymentService.ReqReversal(new Domain.SoapDto.Requests.ReqReversalRequest("asan", "mfyt", "508888888"));
            // var t = paymentService.ReqRefund(new ReqRefundRequest("asan", "mdft", "240914", "1000000"));
            return Ok();
        }
    }
}