using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.StoreProceduresModels.Response;
using bpm.shaparak.ir;
using Microsoft.AspNetCore.Http;
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
        IBamdadPaymentRepository paymentRepository, IAsanRestService asanRestService) : IReturnFromBankService
    {

        public string ReturnUrlRedirectionFromBank(HttpRequest Request)
        {
            string result = "No Bank Found";

            if (Request.Method != "POST") return "Wrong Request";

            string bankCode = GetBankCodeFromUrl(Request);

            if (string.IsNullOrEmpty(bankCode)) return "No Bank Found";

            if (bankCode == nameof(BankCode.Mellat)) return ReturnedFromMellat(Request);

            if (bankCode == nameof(BankCode.Parsian)) return string.Empty;

            if (bankCode == nameof(BankCode.Asan)) result = asanRestService.Return(Request);

            return result;
        }

        public string ReturnedFromMellat(HttpRequest Request)
        {
            string saleOrderId = Request.Form["SaleOrderId"];

            if (string.IsNullOrEmpty(Request.Form["SaleReferenceId"])) return paymentService.CancelPayment(saleOrderId);

            var paymentDetail = paymentRepository.SelectPaymentDetail(new SelectPaymentDetailParameter(saleOrderId));

            if (paymentDetail.Online_Price == 0) return paymentService.FreePayment(saleOrderId);

            string refId = Request.Form["RefId"];
            string saleReferenceId = Request.Form["SaleReferenceId"];
            string resCode = Request.Form["ResCode"];
            string cardHolderInfo = Request.Form["CardHolderInfo"] + "?OnlineID=" + saleOrderId;
            string referenceNumber = null;

            string settleResult = "0";
            string verifyResult = "0";
            string inquieryResult = "0";

            paymentRepository.InsertTransactionResult(new InsertTransactionResultParameter(referenceNumber, Convert.ToInt32(saleOrderId), refId, saleReferenceId, Convert.ToInt32(resCode), cardHolderInfo));

            if (resCode != "0")
                if (string.IsNullOrEmpty(resCode))
                    return FailedPayment(referenceNumber, saleOrderId, refId, saleReferenceId, resCode, cardHolderInfo);

            SelectBankDetailResult BankDetail = paymentRepository.SelectBankDetail(new SelectBankDetailParameter(saleOrderId)).SingleOrDefault();

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

            var url = paymentRepository.UpdateOnlinePayment(new UpdateOnlinePayParameter(referenceNumber, Convert.ToInt32(saleOrderId), refId, saleReferenceId,
                Convert.ToInt32(verifyResult), cardHolderInfo)).Site_ReturnUrl;

            if (paymentDetail.AutoSettle == false) return url;

            settleResult = mellatGatewayService.bpSettleRequest(new bpSettleRequest(new bpSettleRequestBody(
                mellatRequest.merchantID, mellatRequest.BankUser, mellatRequest.BankPass, mellatRequest.OrderID,
                mellatRequest.SaleOrderID, mellatRequest.OrderNo))).Body.@return;

            if (settleResult == "0" || settleResult == "45")
            {
                paymentRepository.UpdateOnlinePayResWithSettle(new UpdateOnlinePayResWithSettleParameter(Convert.ToInt32(saleOrderId)));

                var updateResult = paymentRepository.UpdateOnlinePayment(new UpdateOnlinePayParameter(referenceNumber, Convert.ToInt32(saleOrderId), refId, saleReferenceId, Convert.ToInt32(settleResult), cardHolderInfo));

                if (updateResult.Success == 1) return updateResult.Site_ReturnUrl;
            }

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