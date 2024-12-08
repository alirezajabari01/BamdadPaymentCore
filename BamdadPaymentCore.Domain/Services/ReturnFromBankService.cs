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
using BamdadPaymentCore.Domain.Enums;
using BamdadPaymentCore.Domain.IRepositories;

namespace BamdadPaymentCore.Domain.Services
{
    public class ReturnFromBankService(
        IHttpContextAccessor httpContextAccessor,
        IPaymentService paymentService,
        IPaymentGateway mellatGatewayService,
        IOptions<PaymentGatewaySetting> paymentSetting,
        IBamdadPaymentRepository paymentRepository) : IReturnFromBankService
    {

        public string ReturnUrlRedirectionFromBank(HttpRequest Request)
        {
            //TODO Right Error Response 

            if (Request.Method != "POST") return "Wrong Request";

            string bankCode = GetBankCodeFromUrl(Request);

            if (string.IsNullOrEmpty(bankCode)) return "No Bank Found";

            if (bankCode == nameof(BankCode.Mellat)) return ReturnedFromMellat(Request);

            if (bankCode == nameof(BankCode.Parsian)) return string.Empty;

            if (bankCode == nameof(BankCode.Asan)) return ReturnFromAsanPardalht(Request);

            return paymentService.CancelPayment(Request.Query["invoiceid"]);
        }

        public string ReturnFromAsanPardalht(HttpRequest Request)
            => paymentService.ProcessAsanPardakhtPayment(Request.Query["invoiceid"]);

        public string ReturnedFromMellat(HttpRequest Request)
        {
            string saleOrderId = Request.Form["SaleOrderId"];
            string refId = Request.Form["RefId"];
            string saleReferenceId = Request.Form["SaleReferenceId"];
            string resCode = Request.Form["ResCode"];
            string cardHolderInfo = Request.Form["CardHolderInfo"] + "?OnlineID=" + saleOrderId;
            string referenceNumber = null;

            string settleResult = "0";
            string verifyResult = "0";
            string inquieryResult = "0";

            if (resCode != "0")
                if (string.IsNullOrEmpty(resCode))
                    return FailedPayment(referenceNumber, saleOrderId, refId, saleReferenceId, resCode, cardHolderInfo);

            SelectBankDetailResult BankDetail = paymentRepository
                .SelectBankDetail(new SelectBankDetailParameter(saleOrderId)).SingleOrDefault();

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
                return FailedPayment(referenceNumber, saleOrderId, refId, saleReferenceId, verifyResult,
                    cardHolderInfo);

            settleResult = mellatGatewayService.bpSettleRequest(new bpSettleRequest(new bpSettleRequestBody(
                mellatRequest.merchantID, mellatRequest.BankUser, mellatRequest.BankPass, mellatRequest.OrderID,
                mellatRequest.SaleOrderID, mellatRequest.OrderNo))).Body.@return;

            if (settleResult == "0" || settleResult == "45")
                return paymentRepository.UpdateOnlinePayment(new UpdateOnlinePayParameter(referenceNumber,
                    Convert.ToInt32(saleOrderId), refId, saleReferenceId, Convert.ToInt32(settleResult),
                    cardHolderInfo)).Site_ReturnUrl;

            return FailedPayment(referenceNumber, saleOrderId, refId, saleReferenceId, settleResult, cardHolderInfo);
        }


        #region PrivateMethods

        string GetBankCodeFromUrl(HttpRequest Request)
            => string.IsNullOrEmpty(Request.Form["SaleOrderId"])
                ? paymentRepository.SelectPaymentDetail(new SelectPaymentDetailParameter(Request.Query["invoiceid"]))
                    .BankCode
                : paymentRepository.SelectPaymentDetail(new SelectPaymentDetailParameter(Request.Form["SaleOrderId"]))
                    .BankCode;


        string FailedPayment(string referenceNumber, string saleOrderId, string refId, string saleReferenceId,
            string resCode, string cardHolderInfo)
            => paymentRepository.UpdateOnlinePaymentFailed(new UpdateOnlinePayFailedParameter(referenceNumber,
                    Convert.ToInt32(saleOrderId), refId, saleReferenceId, Convert.ToInt32(resCode), cardHolderInfo))
                .Site_ReturnUrl;

        #endregion
    }
}