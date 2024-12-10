﻿using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.bill;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.reverse;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.settlement;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.verify;
using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.Models.ServicesModels;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;
using bpm.shaparak.ir;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Services
{
    public class MellatService(IBamdadPaymentRepository repository, IPaymentGateway mellatGateway, IPaymentGateway mellatGatewayService,
        IOptions<PaymentGatewaySetting> paymentGatewaySetting) : IMellatService
    {
        public string InquiryTransaction(MellatRequest requst)
        => mellatGatewayService.bpInquiryRequest(new bpInquiryRequest(new bpInquiryRequestBody(requst.terminalId, requst.userName, requst.userPassword, requst.orderId, requst.saleOrderId, requst.saleReferenceId))).Body.@return;

        public string ProcessCallBackFromBank(HttpRequest Request)
        {
            string saleOrderId = Request.Form["SaleOrderId"];

            if (string.IsNullOrEmpty(Request.Form["SaleReferenceId"])) return CancelPayment(saleOrderId);

            var paymentDetail = repository.SelectPaymentDetail(new SelectPaymentDetailParameter(saleOrderId));

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

            repository.InsertTransactionResult(new InsertTransactionResultParameter(referenceNumber, Convert.ToInt32(saleOrderId), refId, saleReferenceId, Convert.ToInt32(resCode), cardHolderInfo));

            SelectBankDetailResult BankDetail = repository.SelectBankDetail(new SelectBankDetailParameter(saleOrderId)).SingleOrDefault();

            var mellatRequest = new MellatRequest
            (
                 long.Parse(BankDetail.Bank_MerchantID),
                 BankDetail.Bank_User,
                 BankDetail.Bank_Pass,
                 long.Parse(saleOrderId),
                 long.Parse(saleOrderId),
                 long.Parse(saleReferenceId)
            );

            verifyResult = VerifyTransaction(mellatRequest);

            if (verifyResult != "0") inquieryResult = InquiryTransaction(mellatRequest);

            if (inquieryResult != "0") return FailedPayment(referenceNumber, saleOrderId, refId, saleReferenceId, verifyResult, cardHolderInfo);

            var url = repository.UpdateOnlinePayment(new UpdateOnlinePayParameter(referenceNumber, Convert.ToInt32(saleOrderId), refId, saleReferenceId,
                Convert.ToInt32(verifyResult), cardHolderInfo)).Site_ReturnUrl;

            if (paymentDetail.AutoSettle == false) return url;

            settleResult = SettleTransaction(mellatRequest);

            if (settleResult == "0" || settleResult == "45")
            {
                repository.UpdateOnlinePayResWithSettle(new UpdateOnlinePayResWithSettleParameter(Convert.ToInt32(saleOrderId)));

                var updateResult = repository.UpdateOnlinePayment(new UpdateOnlinePayParameter(referenceNumber, Convert.ToInt32(saleOrderId), refId, saleReferenceId, Convert.ToInt32(settleResult), cardHolderInfo));

                if (updateResult.Success == 1) return updateResult.Site_ReturnUrl;
            }

            return FailedPayment(referenceNumber, saleOrderId, refId, saleReferenceId, settleResult, cardHolderInfo);
        }

        public string ReverseTransaction(MellatRequest requst)
        => mellatGateway.bpReversalRequest(new bpReversalRequest(new bpReversalRequestBody(requst.terminalId, requst.userName, requst.userPassword, requst.orderId, requst.saleOrderId, requst.saleReferenceId))).Body.@return;

        public string SettleTransaction(MellatRequest requst)
        => mellatGatewayService.bpSettleRequest(new bpSettleRequest(new bpSettleRequestBody(requst.terminalId, requst.userName, requst.userPassword, requst.orderId, requst.saleOrderId, requst.saleReferenceId))).Body.@return;

        public string VerifyTransaction(MellatRequest requst)
        => mellatGatewayService.bpVerifyRequest(new bpVerifyRequest(new bpVerifyRequestBody(requst.terminalId, requst.userName, requst.userPassword, requst.orderId, requst.saleOrderId, requst.saleReferenceId))).Body.@return;

        public string RefundTransaction(MellatRequest request)
       => mellatGatewayService.bpRefundRequest(new bpRefundRequest(new bpRefundRequestBody(request.terminalId, request.userName, request.userPassword, request.orderId, request.saleOrderId, request.saleReferenceId, 0))).Body.@return;

        public string SendToMellatPaymentGateway(SelectPaymentDetailResult paymentDetail, string onlineId)
        {
            DateTime now = DateTime.Now;
            string time = now.ToString("HHmmss");
            string date = now.ToString("yyyyMMdd");

            string mellatResponse = paymentDetail.Online_Kind == 1 || paymentDetail.Online_Kind == 0
            ? PayNormalMelat(paymentDetail, onlineId, date, time, paymentGatewaySetting.Value.MelatReturnBank)
            : PayWithKind(paymentDetail, onlineId, date, time, paymentGatewaySetting.Value.MelatReturnBank);

            string[] strArray = mellatResponse.Split(',');

            if (string.IsNullOrEmpty(mellatResponse) || strArray[0] != "0") return SiteErrorResponse.BankConnectionFailed;

            return strArray[1];
        }

        #region PrivateMethods

        private string FailedPayment(string referenceNumber, string saleOrderId, string refId, string saleReferenceId, string resCode, string cardHolderInfo) => repository.UpdateOnlinePaymentFailed(new UpdateOnlinePayFailedParameter(referenceNumber, Convert.ToInt32(saleOrderId), refId,
                saleReferenceId, Convert.ToInt32(resCode), cardHolderInfo)).Site_ReturnUrl;

        private string CancelPayment(string onlineId)
            => repository.UpdateOnlinePaymentFailed(new UpdateOnlinePayFailedParameter(null, Convert.ToInt32(onlineId), "cancel", "Failed", -1, "use cancel payment")).Site_ReturnUrl;

        private string FreePayment(string saleOrderId) => repository.UpdateOnlinePayment(new UpdateOnlinePayParameter("", Convert.ToInt32(saleOrderId), "Free", "Free", -1, "")).Site_ReturnUrl;

        private string PayNormalMelat(SelectPaymentDetailResult paymentDetail, string reqOnlineId, string date, string time, string ReturnBank)
       => mellatGateway.bpPayRequest(new bpPayRequest(new bpPayRequestBody(
               long.Parse(paymentDetail.Bank_MerchantID.ToString())
               , paymentDetail.Bank_User
               , paymentDetail.Bank_Pass
               , long.Parse(reqOnlineId)
               , Convert.ToInt64(paymentDetail.Online_Price)
               , date
               , time
               , "0"
               , ReturnBank
               , "0"
               , ""
               , ""
               , ""
               , ""
               , null
               ))).Body.@return;

        private string PayWithKind(SelectPaymentDetailResult paymentDetail, string reqOnlineId, string date, string time, string ReturnBank)
        => mellatGateway.bpDynamicPayRequest(new bpDynamicPayRequest(new bpDynamicPayRequestBody(
                long.Parse(paymentDetail.Bank_MerchantID.ToString())
                , paymentDetail.Bank_User.ToString()
                , paymentDetail.Bank_Pass.ToString()
                , long.Parse(reqOnlineId)
                , long.Parse(paymentDetail.Online_Price.ToString())
                , date
                , time
                , "0"
                , ReturnBank
                , "0"
                , long.Parse(paymentDetail.Online_Kind.ToString())
                , ""
                , ""
                , ""
                , ""
                , null))).Body.@return;

        #endregion
    }
}