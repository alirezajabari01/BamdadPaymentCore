using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.bill;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.reverse;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.settlement;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.verify;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.Models.ServicesModels;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;
using bpm.shaparak.ir;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Services
{
    public class MellatService(IBamdadPaymentRepository repository, IPaymentGateway mellatGateway,IPaymentGateway mellatGatewayService) : IMellatService
    {
        public Task<CancelResultVm> CancelTransaction(MellatRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PaymentResultVm> InquiryTransaction(MellatRequest request)
        {
            throw new NotImplementedException();
        }

        public string ProcessCallBackFromBank(HttpRequest Request)
        {
            string saleOrderId = Request.Form["SaleOrderId"];

            if (string.IsNullOrEmpty(Request.Form["SaleReferenceId"])) return CancelPayment(saleOrderId);

            var paymentDetail = repository.SelectPaymentDetail(new SelectPaymentDetailParameter(saleOrderId));

            if (paymentDetail.Online_Price == 0) return repository.UpdateOnlinePayment(new UpdateOnlinePayParameter("", Convert.ToInt32(saleOrderId), "Free", "Free", -1, "")).Site_ReturnUrl;

            string refId = Request.Form["RefId"];
            string saleReferenceId = Request.Form["SaleReferenceId"];
            string resCode = Request.Form["ResCode"];
            string cardHolderInfo = Request.Form["CardHolderInfo"] + "?OnlineID=" + saleOrderId;
            string referenceNumber = null;
            string settleResult = "0";
            string verifyResult = "0";
            string inquieryResult = "0";

            repository.InsertTransactionResult(new InsertTransactionResultParameter(referenceNumber, Convert.ToInt32(saleOrderId), refId, saleReferenceId, Convert.ToInt32(resCode), cardHolderInfo));

            if (resCode != "0")
                if (string.IsNullOrEmpty(resCode))
                    return FailedPayment(referenceNumber, saleOrderId, refId, saleReferenceId, resCode, cardHolderInfo);

            SelectBankDetailResult BankDetail = repository.SelectBankDetail(new SelectBankDetailParameter(saleOrderId)).SingleOrDefault();

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

            var url = repository.UpdateOnlinePayment(new UpdateOnlinePayParameter(referenceNumber, Convert.ToInt32(saleOrderId), refId, saleReferenceId,
                Convert.ToInt32(verifyResult), cardHolderInfo)).Site_ReturnUrl;

            if (paymentDetail.AutoSettle == false) return url;

            settleResult = mellatGatewayService.bpSettleRequest(new bpSettleRequest(new bpSettleRequestBody(
                mellatRequest.merchantID, mellatRequest.BankUser, mellatRequest.BankPass, mellatRequest.OrderID,
                mellatRequest.SaleOrderID, mellatRequest.OrderNo))).Body.@return;

            if (settleResult == "0" || settleResult == "45")
            {
                repository.UpdateOnlinePayResWithSettle(new UpdateOnlinePayResWithSettleParameter(Convert.ToInt32(saleOrderId)));

                var updateResult = repository.UpdateOnlinePayment(new UpdateOnlinePayParameter(referenceNumber, Convert.ToInt32(saleOrderId), refId, saleReferenceId, Convert.ToInt32(settleResult), cardHolderInfo));

                if (updateResult.Success == 1) return updateResult.Site_ReturnUrl;
            }

            return FailedPayment(referenceNumber, saleOrderId, refId, saleReferenceId, settleResult, cardHolderInfo);
        }

        public Task<ReverseVm> ReverseTransaction(MellatRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<SettleVm> SettleTransaction(MellatRequest requst)
        {
            throw new NotImplementedException();
        }

        public Task<VerifyVm> VerifyTransaction(MellatRequest requst)
        {
            throw new NotImplementedException();
        }


        #region PrivateMethods

        private string FailedPayment(string referenceNumber, string saleOrderId, string refId, string saleReferenceId,
            string resCode, string cardHolderInfo) => repository.UpdateOnlinePaymentFailed(new UpdateOnlinePayFailedParameter(referenceNumber, Convert.ToInt32(saleOrderId), refId,
                saleReferenceId, Convert.ToInt32(resCode), cardHolderInfo)).Site_ReturnUrl;

        private string CancelPayment(string onlineId)
            => repository.UpdateOnlinePaymentFailed(new UpdateOnlinePayFailedParameter(null, Convert.ToInt32(onlineId), "cancel", "Failed", -1, "use cancel payment")).Site_ReturnUrl;

        #endregion
    }
}
