using System.Diagnostics;
using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.SoapDto.Requests;
using BamdadPaymentCore.Domain.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BamdadPaymentCore.Controllers
{
    public class HomeController(
        IPaymentService paymentService,
        IHttpContextAccessor httpContextAccessor,
        IBamdadPaymentRepository bamdadPaymentRepository,
        IOptions<PaymentGatewaySetting> paymentGatewaySetting) : ControllerBase
    {
        public IActionResult Index()


        {

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
            //var t = paymentService.GetOnlineIdWithSettle(new GetOnlineIdWithSettleRequest("asan", "mft", "100000", "sdf", "45","0"));
            var t = paymentService.GetOnlineIdDifferentTypes("asan", "mft", "100000", "sdf", "45","0",true);
            //var t = paymentService.GetOnlineStatus(new GetOnlineStatusParameter("asan", "mft", "240914"));
            //var t = paymentService.ReqReversal(new Domain.SoapDto.Requests.ReqReversalRequest("asan", "mfyt", "508888888"));
            // var t = paymentService.ReqRefund(new ReqRefundRequest("asan", "mdft", "240914", "1000000"));
            return Ok();
        }
    }
}