using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.settlement;
using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.Exceptions;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.Models.SoapDto.Requests;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;

using bpm.shaparak.ir;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using XAct.Users;
using XSystem.Security.Cryptography;

namespace BamdadPaymentCore.Domain.Services
{
    public class PaymentService(
        IBamdadPaymentRepository repository,
        IHttpContextAccessor httpContextAccessor,
        IOptions<PaymentGatewaySetting> paymentGatewaySetting,
        IAsanRestService asanRestService) : IPaymentService
    {
        #region PrivateFields

        private const string AuthenticationFailResponse = "-2,Authenticationfailed";
        private const string FillParameterFailResponse = "-3,FillParameter";

        #endregion

        public string GetOnlineIdDifferentTypes(string userName, string password, string onlinePrice,
         string desc, string reqId, string kind, bool autoSettle = false, string onlineType = "payment", string mobleNomber = "")
        {
            SiteAuthenticationResult? siteAuthenticationResult = Authenticate(userName, password);
            if (siteAuthenticationResult is null) return AuthenticationFailResponse;

            var bankId = repository
                .SelectBankID(new SelectBankIdParameter(Convert.ToInt32(siteAuthenticationResult.Site_ID))).Bank_ID;

            return string.IsNullOrEmpty(onlinePrice)
                ? FillParameterFailResponse
                : repository.InsertOnlinePay(new InsertIntoOnlinePayParameter(bankId, siteAuthenticationResult.Site_ID,
                        ConvertToInt(onlinePrice), desc, ConvertToInt(reqId), ConvertToInt(kind), autoSettle, onlineType, mobleNomber)).OnlineID.ToString();
        }

        public string CancelPayment(string onlineId)
            => repository.UpdateOnlinePaymentFailed(new UpdateOnlinePayFailedParameter(null, ConvertToInt(onlineId),
                    "cancel", "Failed", -1, "use cancel payment"))
                .Site_ReturnUrl;

        public DataTable GetOnlineStatus(GetOnlineStatusParameter request)
            => Authenticate(request.Username, request.Password) is null
                ? Authenticationfailed()
                : repository.SelectOnlinePay(new SelectOnlinePayParameter(request.OnlineId)).ListToDataTable();

        public bool ReqRefund(ReqRefundRequest request)
            => Authenticate(request.Username, request.Password) is not null ? asanRestService.Cancel(request.OnlineId) : false;

        public DataTable ReqSettleOnline(ReqSettleOnlineRequest request)
        {
            var authResult = Authenticate(request.Username, request.Password);
            if (authResult is not null && string.IsNullOrEmpty(authResult.Site_ReturnUrl))
                return Authenticationfailed();

            var paymentDetail = repository.SelectPaymentDetail(new SelectPaymentDetailParameter(request.OnlineId));

            if (paymentDetail is null || paymentDetail.Online_Status == false || paymentDetail.Online_Price == 0)
                throw new PaymentDetailException();

            var tranResult = asanRestService.GetTransationResultFromAsanPardakht(request.OnlineId, paymentDetail);

            if (tranResult.resCode != 0) return Authenticationfailed();

            var settleResult = asanRestService.SettleAsan(tranResult, paymentDetail, request.OnlineId);

            var updateResult = repository.UpdateOnlinePayment(new UpdateOnlinePayParameter(tranResult.referenceNumber,
              ConvertToInt(request.OnlineId), tranResult.refId, tranResult.saleReferenceId, settleResult.ResCode, tranResult.cardHolderInfo));

            if (updateResult.Success == 0)
                throw new UpdateOnlinePaymentException();

            return repository.SelectOnlinePay(new SelectOnlinePayParameter(request.OnlineId)).ListToDataTable();
        }

        public string UpdateOnlinePayFailed(string referenceNumber, string onlineId, string transactionNo,
            string orderNo, string errorCode, string cardHolderInfo)
            => repository.UpdateOnlinePaymentFailed(new UpdateOnlinePayFailedParameter(referenceNumber,
                    ConvertToInt(onlineId), transactionNo, orderNo, ConvertToInt(errorCode), cardHolderInfo))
                .Site_ReturnUrl;

        public void InsertSiteError(InsertSiteErrorParameter request)
            => repository.insertSiteError(request);

        public string FreePayment(string onlineId)
            => repository
                .UpdateOnlinePayment(new UpdateOnlinePayParameter("", ConvertToInt(onlineId), "Free", "Free", -1, ""))
                .Site_ReturnUrl;

        public bool ReqVerify(VerifyRequest request)
        {
            SiteAuthenticationResult? siteAuthenticationResult = Authenticate(request.Username, request.Password);
            if (siteAuthenticationResult is null) return false;

            var paymentDetail = repository.SelectPaymentDetail(new SelectPaymentDetailParameter(request.OnlineId));

            if (paymentDetail is null) throw new PaymentDetailException();

            var tranReq = new TransactionResultRequest
            (
                paymentGatewaySetting.Value.AsanMerchantId,
                request.OnlineId,
                paymentDetail.Bank_User,
                paymentDetail.Bank_Pass
            );
            var transactionResult = asanRestService.TransactionResult(tranReq).Result;

            repository.InsertTransactionResult(new InsertTransactionResultParameter(transactionResult.Rrn,
                ConvertToInt(request.OnlineId), transactionResult.RefId, transactionResult.PayGateTranID.ToString(),
                transactionResult.ResCode, transactionResult.CardNumber));

            if (transactionResult.ResCode != 0)
            {
                repository.UpdateOnlinePaymentFailed(new UpdateOnlinePayFailedParameter(transactionResult.Rrn,
                    ConvertToInt(request.OnlineId), transactionResult.RefId, transactionResult.PayGateTranID.ToString(),
                    transactionResult.ResCode, transactionResult.CardNumber));
                throw new GetTransationResultException();
            }

            var verifyCommand = new AsanRestRequest
            {
                merchantConfigurationId = Convert.ToInt32(paymentDetail.Bank_MerchantID),
                payGateTranId = Convert.ToUInt64(transactionResult.PayGateTranID),
                BankUser = paymentDetail.Bank_User,
                BankPassword = paymentDetail.Bank_Pass,
            };
            var verifyRes = asanRestService.VerifyTransaction(verifyCommand).Result;

            if (verifyRes.ResCode != 0)
            {
                repository.UpdateOnlinePaymentFailed(new UpdateOnlinePayFailedParameter(transactionResult.Rrn,
                    ConvertToInt(request.OnlineId), transactionResult.RefId, transactionResult.PayGateTranID.ToString(),
                    verifyRes.ResCode, transactionResult.CardNumber));
                throw new VerifyTransationException();
            }

            repository.UpdateOnlinePayment(new UpdateOnlinePayParameter(transactionResult.Rrn,
                ConvertToInt(request.OnlineId), transactionResult.RefId, transactionResult.PayGateTranID.ToString(),
                verifyRes.ResCode, transactionResult.CardNumber));

            return true;
        }

        public DataTable RequestSettleOnline(SettleOnlineRequest request)
        {
            SiteAuthenticationResult? siteAuthenticationResult = Authenticate(request.Username, request.Password);
            if (siteAuthenticationResult is null) return Authenticationfailed();

            //asanRestService.ProcessAsanPardakhtPayment(request.OnlineId);

            return GetOnlineStatus(new GetOnlineStatusParameter(request.Username, request.Password, request.OnlineId));
        }

        public bool RequestReversal(string username, string pass, string onlineId)
        {
            var paymentDetail = repository.SelectPaymentDetail(new SelectPaymentDetailParameter(onlineId));

            if (paymentDetail.Online_Status == true) throw new PaymentDetailException();

            var tranReq = new TransactionResultRequest
            (
                paymentGatewaySetting.Value.AsanMerchantId,
                onlineId,
                paymentDetail.Bank_User,
                paymentDetail.Bank_Pass
            );

            var transactionResult = asanRestService.TransactionResult(tranReq).Result;

            if (transactionResult.ResCode != 0) throw new GetTransationResultException();

            var reverseCommand = new AsanRestRequest()
            {
                merchantConfigurationId = ConvertToInt(paymentDetail.Bank_MerchantID),
                payGateTranId = Convert.ToUInt64(transactionResult.PayGateTranID),
                BankUser = paymentDetail.Bank_User,
                BankPassword = paymentDetail.Bank_Pass,
            };

            var reverseResult = asanRestService.ReverseTransaction(reverseCommand).Result;

            if (reverseResult.ResCode != 0) throw new ReverseTransationException();

            repository.UpdateOnlinePayReversal(new UpdateOnlinePayReversalParameter(ConvertToInt(onlineId),
                ConvertToInt(reverseResult.ResCode.ToString())));

            return true;
        }


        #region PrivateMethods

        private DataTable GetOnlineStatus(string onlineId)
            => repository.SelectOnlinePay(new SelectOnlinePayParameter(onlineId)).ListToDataTable();

        private string GetSiteIp() => httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();

        private int ConvertToInt(string value) => Convert.ToInt32(value);

        private SiteAuthenticationResult Authenticate(string username, string password)
            => repository.SelectSiteAuthentication(new SiteAuthenticationParameter(username, Helper.HashMd5(password),
                GetSiteIp()));



        public DataTable Authenticationfailed()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ErrorId", typeof(string));
            dataTable.Columns.Add("ErrorName", typeof(string));
            DataRow row = dataTable.NewRow();
            row["ErrorId"] = (object)"-2";
            row["ErrorName"] = (object)nameof(Authenticationfailed);
            dataTable.Rows.Add(row);
            dataTable.TableName = "error";
            return dataTable;
        }

        //private bool ReversalRequest(string onlineId)
        //{
        //    bool result = false;
        //    string errorCode = string.Empty;

        //    SelectBankDetailResult SelectBankDetailResult =
        //        repository.SelectBankDetail(new SelectBankDetailParameter(onlineId)).Single();

        //    SelectOnlinePayDetailResult SelectOnlinePayDetailResult = repository
        //        .SelectOnlinePayDetail(new SelectOnlinePayDetailParameter(onlineId)).First();

        //    if (SelectBankDetailResult is not null || SelectOnlinePayDetailResult is not null)
        //    {
        //        if (SelectOnlinePayDetailResult.Online_Status == false)
        //        {
        //            errorCode = mellatGateway.bpReversalRequest(new bpReversalRequest(new bpReversalRequestBody
        //                (long.Parse(SelectBankDetailResult.Bank_MerchantID), SelectBankDetailResult.Bank_User,
        //                    SelectBankDetailResult.Bank_Pass, long.Parse(onlineId), long.Parse(onlineId),
        //                    long.Parse(SelectOnlinePayDetailResult.Online_OrderNo.ToString()))
        //            )).Body.@return;

        //            repository.UpdateOnlinePayReversal(
        //                new UpdateOnlinePayReversalParameter(ConvertToInt(onlineId), ConvertToInt(errorCode)));
        //            result = errorCode == "0";
        //        }
        //    }

        //    return result;
        //}

        //public bool RefundRequest(string onlineId, string refundAmount)
        //{
        //    bool result = false;
        //    string errorCode = string.Empty;

        //    SelectBankDetailResult SelectBankDetailResult = repository.SelectBankDetail(new SelectBankDetailParameter(onlineId)).SingleOrDefault();

        //    SelectOnlinePayDetailResult SelectOnlinePayDetailResult = repository.SelectOnlinePayDetail(new SelectOnlinePayDetailParameter(onlineId)).FirstOrDefault();

        //    if (SelectBankDetailResult is not null || SelectOnlinePayDetailResult is not null)
        //    {
        //        if (SelectOnlinePayDetailResult.Online_Status == false)
        //        {
        //            long amount = Helper.RefundAmount(long.Parse(refundAmount), SelectOnlinePayDetailResult);

        //            var insertResult = repository.InsertOnlinePay(new InsertOnlinePayParameter
        //            (
        //                ConvertToInt(SelectBankDetailResult.Bank_ID.ToString()),
        //                ConvertToInt(SelectBankDetailResult.Site_ID.ToString()),
        //                ConvertToInt(amount.ToString()),
        //                onlineId,
        //                ConvertToInt(SelectOnlinePayDetailResult.Online_ReqID.ToString()),
        //                ConvertToInt(SelectOnlinePayDetailResult.Online_Kind.ToString()),
        //                1,
        //                 "refund")
        //            );

        //            errorCode = mellatGateway.bpRefundRequest(new bpRefundRequest(new bpRefundRequestBody(
        //               long.Parse(SelectBankDetailResult.Bank_MerchantID), SelectBankDetailResult.Bank_User, SelectBankDetailResult.Bank_Pass, long.Parse(insertResult.OnlineID.ToString()), long.Parse(onlineId), long.Parse(SelectOnlinePayDetailResult.Online_OrderNo), amount))).Body.@return;

        //            repository.UpdateOnlinePayRefund(new UpdateOnlinePayRefundParameter(insertResult.OnlineID.ToString(), errorCode));

        //            result = errorCode == "0";
        //        }
        //    }
        //    return result;
        //}



        //public bool SettleRequest(string onlineId)
        //{
        //    //TODO SiteError

        //    bool result = false;
        //    string settleResult = "0";
        //    string verifyResult = "0";
        //    string inquieryResult = "0";

        //    var SelectBankDetailResult =
        //        repository.SelectBankDetail(new SelectBankDetailParameter(onlineId)).FirstOrDefault();

        //    var SelectOnlinePayDetailResult = repository
        //        .SelectOnlinePayDetail(new SelectOnlinePayDetailParameter(onlineId)).FirstOrDefault();

        //    var mellatRequest = new
        //    {
        //        merchantID = long.Parse(SelectBankDetailResult.Bank_MerchantID),
        //        BankUser = SelectBankDetailResult.Bank_User,
        //        BankPass = SelectBankDetailResult.Bank_Pass,
        //        OrderID = long.Parse(onlineId),
        //        SaleOrderID = long.Parse(onlineId),
        //        OrderNo = long.Parse(SelectOnlinePayDetailResult.Online_OrderNo)
        //    };

        //    if (SelectBankDetailResult is not null || SelectOnlinePayDetailResult is not null)
        //    {
        //        if (SelectOnlinePayDetailResult.IsSettle != 1)
        //        {
        //            verifyResult = mellatGateway.bpVerifyRequest(new bpVerifyRequest(new bpVerifyRequestBody(
        //                mellatRequest.merchantID, mellatRequest.BankUser, mellatRequest.BankPass, mellatRequest.OrderID,
        //                mellatRequest.SaleOrderID, mellatRequest.OrderNo))).Body.@return;

        //            if (verifyResult != "0")
        //            {
        //                inquieryResult = mellatGateway.bpInquiryRequest(new bpInquiryRequest(new bpInquiryRequestBody(
        //                    mellatRequest.merchantID, mellatRequest.BankUser, mellatRequest.BankPass,
        //                    mellatRequest.OrderID,
        //                    mellatRequest.SaleOrderID, mellatRequest.OrderNo))).Body.@return;
        //            }

        //            if (inquieryResult != "0")
        //            {
        //                if (verifyResult == "0" || inquieryResult == "0")
        //                    repository.UpdateOnlinePayResWithSettle(
        //                        new UpdateOnlinePayResWithSettleParameter(ConvertToInt(onlineId)));

        //                repository.UpdateOnlinePaySettleFailed(
        //                    new UpdateOnlinePayResWithSettleFailedParameter(onlineId, ConvertToInt(verifyResult)));
        //            }
        //            else
        //            {
        //                settleResult = mellatGateway.bpSettleRequest(new bpSettleRequest(new bpSettleRequestBody(
        //                    mellatRequest.merchantID, mellatRequest.BankUser, mellatRequest.BankPass,
        //                    mellatRequest.OrderID,
        //                    mellatRequest.SaleOrderID, mellatRequest.OrderNo))).Body.@return;
        //            }

        //            result = true;
        //        }
        //    }

        //    return result;
        //}


        #endregion
    }
}