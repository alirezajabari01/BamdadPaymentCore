using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.StoreProceduresModels.Response;
using bpm.shaparak.ir;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestService.models.settle;
using RestService.models.verify;
using RestService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using BamdadPaymentCore.Domain.Common;

namespace BamdadPaymentCore.Domain.Services
{
    public class ReturnFromBankService(IHttpContextAccessor httpContextAccessor, IPaymentService paymentService, IPaymentGateway mellatGatewayService, IOptions<PaymentGatewaySetting> paymentSetting, IIPGResetService _ipgService) : IReturnFromBankService
    {
        public string ReturnUrlRedirectionFromBank(HttpRequest Request)
        {
            if (Request.Method != HttpMethod.Get.ToString()) return "Fail";
            
            if (string.IsNullOrEmpty(Request.Form["SaleOrderId"])) return ReturnedFromMellat(httpContextAccessor.HttpContext.Request);

            if (string.IsNullOrEmpty(Request.Query["invoiceid"])) return string.Empty;

            if (string.IsNullOrEmpty(Request.Form["PaygateTranId"])) return ReturnedFromMellat(Request); 

            return paymentService.UpdateOnlinePayFailed(Request.Query["invoiceid"], "cancel", null, "-1", "use cancel payment");
        }

        public string ReturnFromAsanPardalht(HttpRequest Request)
        {

            string localInvoiceId = Request.Query["invoiceid"];

            if (string.IsNullOrEmpty(localInvoiceId)) return SiteErrorResponse.NullOrEmptyOnlineId;

            var paymentDetail = paymentService.SelectPaymentDetail(new SelectPaymentDetailParameter(localInvoiceId));

            if (paymentDetail is null) return SiteErrorResponse.PaymentNotValid;

            //Free Payment
            if (paymentDetail.Online_Price == 0) return paymentService.UpdateOnlinePay(localInvoiceId, "Free", "Free", "-1", "");

            var tranResult = _ipgService.TranResult(Convert.ToInt32(paymentSetting.Value.AsanMerchantId), Convert.ToInt64(localInvoiceId), paymentDetail).Result;

            VerifyVm verifyRes = null;
            SettleVm settleRes = null;

            string saleOrderId = localInvoiceId;
            string refId = tranResult.RefId;
            string saleReferenceId = tranResult.PayGateTranID.ToString();
            string cardHolderInfo = Request.Form["CardHolderInfo"] + "?OnlineID=" + saleOrderId;

            if (tranResult.ResCode != 0)
                return paymentService.UpdateOnlinePayFailed(saleOrderId, refId, saleReferenceId, tranResult.ResCode.ToString(), cardHolderInfo);


            var verifyCommand = new VerifyCommand()
            {
                merchantConfigurationId = Convert.ToInt32(paymentDetail.Bank_MerchantID.ToString()),
                payGateTranId = Convert.ToUInt64(tranResult.PayGateTranID)
            };
            verifyRes = _ipgService.VerifyTrx(verifyCommand, paymentDetail).Result;

            if (verifyRes.ResCode != 0) return paymentService.UpdateOnlinePayFailed(saleOrderId, refId, saleReferenceId, verifyRes.ResCode.ToString(), cardHolderInfo);

            var settleCommand = new SettleCommand()
            {
                merchantConfigurationId = int.Parse(paymentSetting.Value.AsanMerchantId),
                payGateTranId = Convert.ToUInt64(tranResult.PayGateTranID)
            };
            settleRes = _ipgService.SettleTrx(settleCommand, paymentDetail).Result;

            if (settleRes.ResCode != 0) return paymentService.UpdateOnlinePayFailed(saleOrderId, refId, saleReferenceId, settleRes.ResCode.ToString(), cardHolderInfo);

            //paymentService.UpdateOnlinePayResWithSettle(localInvoiceId);

            //this.lblmessageresult.Text = "تراكنش با موفقيت انجام شد";
            return paymentService.UpdateOnlinePay(saleOrderId, refId, saleReferenceId, settleRes.ResCode.ToString(), cardHolderInfo);
        }

        public string ReturnedFromMellat(HttpRequest Request)
        {
            string saleOrderId = Request.Form["SaleOrderId"];
            string refId = Request.Form["RefId"];
            string saleReferenceId = Request.Form["SaleReferenceId"];
            string resCode = Request.Form["ResCode"];
            string cardHolderInfo = Request.Form["CardHolderInfo"] + "?OnlineID=" + saleOrderId;

            string settleResult = "0";
            string verifyResult = "0";
            string inquieryResult = "0";

            if (resCode != "0") if (string.IsNullOrEmpty(resCode))
                    return paymentService.UpdateOnlinePayFailed(saleOrderId, refId, saleReferenceId, resCode, cardHolderInfo);

            SelectBankDetailResult BankDetail = paymentService.SelectBankDetail(new SelectBankDetailParameter(saleOrderId));

            var mellatRequest = new
            {
                merchantID = long.Parse(BankDetail.Bank_MerchantID),
                BankUser = BankDetail.Bank_User,
                BankPass = BankDetail.Bank_Pass,
                OrderID = long.Parse(saleOrderId),
                SaleOrderID = long.Parse(saleOrderId),
                OrderNo = long.Parse(saleReferenceId)
            };

            verifyResult = mellatGatewayService.bpVerifyRequest(new bpVerifyRequest(new bpVerifyRequestBody(
                  mellatRequest.merchantID, mellatRequest.BankUser, mellatRequest.BankPass, mellatRequest.OrderID,
                  mellatRequest.SaleOrderID, mellatRequest.OrderNo))).Body.@return;

            if (verifyResult != "0")
                inquieryResult = mellatGatewayService.bpInquiryRequest(new bpInquiryRequest(new bpInquiryRequestBody(
                     mellatRequest.merchantID, mellatRequest.BankUser, mellatRequest.BankPass, mellatRequest.SaleOrderID,
                     mellatRequest.SaleOrderID, mellatRequest.OrderNo))).Body.@return;

            if (inquieryResult != "0")
                return paymentService.UpdateOnlinePayFailed(saleOrderId, refId, saleReferenceId, verifyResult, cardHolderInfo);

            settleResult = mellatGatewayService.bpSettleRequest(new bpSettleRequest(new bpSettleRequestBody(
                 mellatRequest.merchantID, mellatRequest.BankUser, mellatRequest.BankPass, mellatRequest.OrderID,
                 mellatRequest.SaleOrderID, mellatRequest.OrderNo))).Body.@return;

            if (settleResult == "0" || settleResult == "45")
                return paymentService.UpdateOnlinePay(saleOrderId, refId, saleReferenceId, settleResult, cardHolderInfo);


            return paymentService.UpdateOnlinePayFailed(saleOrderId, refId, saleReferenceId, settleResult, cardHolderInfo);
        }

        public record ReturnFromBankViewModel(string RefId, string saleReferenceId, string resCode, string Message);
    }
}
