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
    public class ReturnFromBankService(IHttpContextAccessor httpContextAccessor, IPaymentService paymentService, IPaymentGateway mellatGatewayService,IOptions<PaymentGatewaySetting> paymentSetting,IIPGResetService _ipgService) : IReturnFromBankService
    {
        public string ReturnUrlRedirectionFromBank(string bankType)
        {
            if (bankType == "SaleOrderId") return ReturnedFromMellat(httpContextAccessor.HttpContext.Request);
            if (bankType != "invoiceid") return string.Empty;
            if (bankType == "PaygateTranId") return "";

            return string.Empty;
        }

        public string ReturnFromAsanPardalht(HttpRequest Request)
        {
            string localInvoiceId = Request.Query["invoiceid"];

            if (string.IsNullOrEmpty(localInvoiceId)) return "اطلاعات کامل نمی باشد با مدیر سیستم تماس حاصل نمایید.";


            var paymentDetail = paymentService.SelectPaymentDetail(new SelectPaymentDetailParameter(localInvoiceId));

            if (paymentDetail is null) return "ارتباط معتبر نمی باشد با مدیر سیستم تماس حاصل نمایید.";

            if (paymentDetail.Online_Price == 0) return paymentService.UpdateOnlinePay(localInvoiceId, "Free", "Free", "-1", "");


            var paymentResult = _ipgService.TranResult(
                                                        Convert.ToInt32(paymentSetting.Value.AsanMerchantId),
                                                        Convert.ToInt64(localInvoiceId), paymentDetail).Result;

           
            VerifyVm verifyRes = null;
            SettleVm settleRes = null;

            //if (paymentResult.ResCode != 0)
            //    var errorCode = settleRes != null ? settleRes.ResCode : verifyRes != null ? verifyRes.ResCode : paymentResult.ResCode;
            //this.Response.Redirect(paymentService.UpdateOnlinePayFailed(this.Request.Params["SaleOrderId"], this.Request.Params["RefId"], this.Request.Params["SaleReferenceId"], errorCode.ToString(), this.Request.Params["CardHolderInfo"]) + "?OnlineID=" + this.SaleOrderIdLabel.Text);
            

                var verifyCommand = new VerifyCommand()
                {
                    merchantConfigurationId = Convert.ToInt32(paymentDetail.Bank_MerchantID.ToString()),
                    payGateTranId = Convert.ToUInt64(paymentResult.PayGateTranID)
                };
                verifyRes = _ipgService.VerifyTrx(verifyCommand, paymentDetail).Result;

                if (verifyRes.ResCode == 0)
                {
                    var ipgService1 = new IPGResetService();
                    settleRes = ipgService1.SettleTrx(
                       new SettleCommand()
                       {
                           merchantConfigurationId = int.Parse(paymentSetting.Value.AsanMerchantId),
                           payGateTranId = Convert.ToUInt64(paymentResult.PayGateTranID)
                       }, paymentDetail)
                       .Result;

                    if (settleRes.ResCode == 0)
                    {
                        //new OnlinePay().UpdateOnlinePayResWithSettle(localInvoiceId);
                        ////this.lblmessageresult.Text = "تراكنش با موفقيت انجام شد";
                        //return paymentService.UpdateOnlinePay(this.SaleOrderIdLabel.Text, this.RefIdLabel.Text, this.SaleReferenceIdLabel.Text, settleRes.ResCode.ToString(), this.Request.Params["CardHolderInfo"]) + "?OnlineID=" + this.SaleOrderIdLabel.Text);
                       // return;
                    }

                }

            
          

        }

        public string ReturnedFromMellat(HttpRequest Request)
        {
            string saleOrderId = Request.Query["SaleOrderId"];
            string refId = Request.Query["RefId"];
            string saleReferenceId = Request.Query["SaleReferenceId"];
            string resCode = Request.Query["ResCode"];

            string settleResult = "0";
            string verifyResult = "0";
            string inquieryResult = "0";

            if (resCode != "0") if (string.IsNullOrEmpty(resCode))
                    return paymentService.UpdateOnlinePayFailed(saleOrderId, refId, saleReferenceId, resCode, Request.Query["CardHolderInfo"] + "?OnlineID=" + saleOrderId);

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
                return paymentService.UpdateOnlinePayFailed(saleOrderId, refId, saleReferenceId, verifyResult, Request.Query["CardHolderInfo"] + "?OnlineID=" + saleOrderId);

            settleResult = mellatGatewayService.bpSettleRequest(new bpSettleRequest(new bpSettleRequestBody(
                 mellatRequest.merchantID, mellatRequest.BankUser, mellatRequest.BankPass, mellatRequest.OrderID,
                 mellatRequest.SaleOrderID, mellatRequest.OrderNo))).Body.@return;

            if (settleResult == "0" || settleResult == "45")
                return paymentService.UpdateOnlinePay(saleOrderId, refId, saleReferenceId, settleResult, Request.Query["CardHolderInfo"] + "?OnlineID=" + saleOrderId);


            return paymentService.UpdateOnlinePayFailed(saleOrderId, refId, saleReferenceId, settleResult, Request.Query["CardHolderInfo"] + "?OnlineID=" + saleOrderId);
        }

        public record ReturnFromBankViewModel(string RefId, string saleReferenceId, string resCode, string Message);
    }
}
