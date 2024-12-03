using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.Exceptions;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.SoapDto.Requests;
using BamdadPaymentCore.Domain.StoreProceduresModels;
using BamdadPaymentCore.Domain.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.StoreProceduresModels.Response;
using bpm.shaparak.ir;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Options;
using Nobisoft.Core.Extensions;
using RestService.models.reverse;
using RestService.models.settle;
using RestService.models.verify;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using XAct.Users;
using XSystem.Security.Cryptography;

namespace BamdadPaymentCore.Domain.Services
{
    public class PaymentService(IBamdadPaymentRepository repository, IHttpContextAccessor httpContextAccessor, IPaymentGateway mellatGateway,
        IAsanResetService asanRestService, IOptions<PaymentGatewaySetting> paymentGatewaySetting) : IPaymentService
    {
        #region PrivateFields

        private const string AuthenticationFailResponse = "-2,Authenticationfailed";
        private const string FillParameterFailResponse = "-3,FillParameter";

        #endregion

        public string CancelPayment(string onlineId)
            => repository.UpdateOnlinePaymentFailed(new UpdateOnlinePayFailedParameter(null, ConvertToInt(onlineId), "cancel", "Failed", -1, "use cancel payment"))
            .Site_ReturnUrl;

        public string GetOnlineId(GetOnlineIdRequest request)
        => GetOnlineIdDifferentTypes(request.Username, request.Password, request.Price, request.Desc, request.ReqId, "0");

        public string GetOnlineIdkind(GetOnlineIdkindRequest request)
        => GetOnlineIdDifferentTypes(request.Username, request.Password, request.Price, request.Desc, request.ReqId, request.Kind);

        public string GetOnlineIdWithSettle(GetOnlineIdWithSettleRequest request)
        => GetOnlineIdDifferentTypes(request.Username, request.Password, request.Price, request.Desc, request.ReqId, request.Kind, 1);

        public DataTable GetOnlineStatus(GetOnlineStatusParameter request)
        => Authenticate(request.Username, request.Password) is null
            ? Authenticationfailed()
            : repository.SelectOnlinePay(new SelectOnlinePayParameter(request.OnlineId)).ListToDataTable();

        public bool ReqRefund(ReqRefundRequest request)
        => Authenticate(request.Username, request.Password) is not null ? RefundRequest(request.OnlineId, request.RefundAmount) : false;

        public bool ReqReversal(ReqReversalRequest request)
        => Authenticate(request.Username, request.Password) is not null ? ReversalRequest(request.OnlineId) : false;

        public DataTable ReqSettleOnline(ReqSettleOnlineRequest request)
        {
            var authResult = Authenticate(request.Username, request.Password);
            if (authResult is not null && string.IsNullOrEmpty(authResult.Site_ReturnUrl)) return Authenticationfailed();

            SettleRequest(request.OnlineId);
            return repository.SelectOnlinePay(new SelectOnlinePayParameter(request.OnlineId)).ListToDataTable();
        }

        public string UpdateOnlinePayFailed(string referenceNumber, string onlineId, string transactionNo, string orderNo, string errorCode, string cardHolderInfo)
      => repository.UpdateOnlinePaymentFailed(new UpdateOnlinePayFailedParameter(referenceNumber, ConvertToInt(onlineId), transactionNo, orderNo, ConvertToInt(errorCode), cardHolderInfo)).Site_ReturnUrl;

        public void InsertSiteError(InsertSiteErrorParameter request)
        => repository.insertSiteError(request);

        public string FreePayment(string onlineId)
        => repository.UpdateOnlinePayment(new UpdateOnlinePayParameter("", ConvertToInt(onlineId), "Free", "Free", -1, "")).Site_ReturnUrl;

        public bool ReqVerify(VerifyRequest request)
        {
            SiteAuthenticationResult? siteAuthenticationResult = Authenticate(request.Username, request.Password);
            if (siteAuthenticationResult is null) return false;

            var paymentDetail = repository.SelectPaymentDetail(new SelectPaymentDetailParameter(request.OnlineId));
            var transactionResult = asanRestService.TransactionResult(ConvertToInt(paymentGatewaySetting.Value.AsanMerchantId), long.Parse(request.OnlineId), paymentDetail).Result;

            if (transactionResult.ResCode != 0) return false;

            var verifyCommand = new VerifyCommand()
            {
                merchantConfigurationId = Convert.ToInt32(paymentDetail.Bank_MerchantID.ToString()),
                payGateTranId = Convert.ToUInt64(transactionResult.PayGateTranID)
            };
            var verifyRes = asanRestService.VerifyTransaction(verifyCommand, paymentDetail).Result;

            if (verifyRes.ResCode != 0) return false;

            return true;
        }

        public DataTable RequestSettleOnline(SettleOnlineRequest request)
        {
            SiteAuthenticationResult? siteAuthenticationResult = Authenticate(request.Username, request.Password);
            if (siteAuthenticationResult is null) return Authenticationfailed();

            Settle(request.OnlineId);

            return GetOnlineStatus(new GetOnlineStatusParameter(request.Username, request.Password, request.OnlineId));

        }

        public string Settle(string onlineId)
        {
            string localInvoiceId = onlineId;

            if (string.IsNullOrEmpty(localInvoiceId)) return SiteErrorResponse.NullOrEmptyOnlineId;

            var paymentDetail = repository.SelectPaymentDetail(new SelectPaymentDetailParameter(localInvoiceId));

            if (paymentDetail is null) return SiteErrorResponse.PaymentNotValid;

            //Free Payment
            if (paymentDetail.Online_Price == 0) return FreePayment(localInvoiceId);

            var tranResult = asanRestService.TransactionResult(Convert.ToInt32(paymentGatewaySetting.Value.AsanMerchantId), Convert.ToInt64(localInvoiceId), paymentDetail).Result;

            string saleOrderId = localInvoiceId;
            string refId = tranResult.RefId;
            string saleReferenceId = tranResult.PayGateTranID.ToString();
            string cardHolderInfo = tranResult.CardNumber;
            string referenceNumber = tranResult.Rrn;

            if (tranResult.ResCode != 0)
                return UpdateOnlinePayFailed(referenceNumber, saleOrderId, refId, saleReferenceId, tranResult.ResCode.ToString(), cardHolderInfo);


            var verifyCommand = new VerifyCommand()
            {
                merchantConfigurationId = Convert.ToInt32(paymentDetail.Bank_MerchantID.ToString()),
                payGateTranId = Convert.ToUInt64(tranResult.PayGateTranID)
            };
            VerifyVm verifyRes = asanRestService.VerifyTransaction(verifyCommand, paymentDetail).Result;

            if (verifyRes.ResCode != 0) return UpdateOnlinePayFailed(referenceNumber, saleOrderId, refId, saleReferenceId,
                verifyRes.ResCode.ToString(), cardHolderInfo);

            var settleCommand = new SettleCommand()
            {
                merchantConfigurationId = int.Parse(paymentGatewaySetting.Value.AsanMerchantId),
                payGateTranId = Convert.ToUInt64(tranResult.PayGateTranID)
            };
            SettleVm settleRes = asanRestService.SettleTransaction(settleCommand, paymentDetail).Result;

            if (settleRes.ResCode != 0) return UpdateOnlinePayFailed(referenceNumber, saleOrderId, refId, saleReferenceId,
                settleRes.ResCode.ToString(), cardHolderInfo);

            repository.UpdateOnlinePayResWithSettle(new UpdateOnlinePayResWithSettleParameter(localInvoiceId));

            return repository.UpdateOnlinePayment(new UpdateOnlinePayParameter(referenceNumber, ConvertToInt(saleOrderId), refId, saleReferenceId, settleRes.ResCode, cardHolderInfo)).Site_ReturnUrl;
        }

        public bool RequestReversal(string username, string pass, string onlineId)
        {
            var paymentDetail = repository.SelectPaymentDetail(new SelectPaymentDetailParameter(onlineId));

            var transactionResult = asanRestService.TransactionResult(Convert.ToInt32(paymentGatewaySetting.Value.AsanMerchantId), long.Parse(onlineId), paymentDetail)
                .Result;

            if (transactionResult.ResCode != 0) throw new Exception(transactionResult.ResMessage);

            var reverseResult = asanRestService.ReverseTransaction(new ReverseCommand(), paymentDetail.Bank_User, paymentDetail.Bank_Pass).Result;

            if(reverseResult.ResCode != 0) throw new Exception(reverseResult.ResMessage);

            return true;
        }

        #region PrivateMethods

        private DataTable GetOnlineStatus(string onlineId)
            => repository.SelectOnlinePay(new SelectOnlinePayParameter(onlineId)).ListToDataTable();

        private string GetSiteIp() => httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();

        private int ConvertToInt(string value) => Convert.ToInt32(value);

        private SiteAuthenticationResult Authenticate(string username, string password)
           => repository.SelectSiteAuthentication(new SiteAuthenticationParameter(username, Helper.HashMd5(password), GetSiteIp()));

        private string GetOnlineIdDifferentTypes(string userName, string password, string onlinePrice,
        string desc, string reqId, string kind, int isSettle = 0, string onlineType = "payment")
        {
            SiteAuthenticationResult? siteAuthenticationResult = Authenticate(userName, password);
            if (siteAuthenticationResult is null) return AuthenticationFailResponse;

            var bankId = repository.SelectBankID
                     (new SelectBankIdParameter(Convert.ToInt32(siteAuthenticationResult.Site_ID))).Bank_ID;

            return !string.IsNullOrEmpty(onlinePrice) ?
                repository.InsertOnlinePay(new InsertOnlinePayParameter(bankId, siteAuthenticationResult.Site_ID, ConvertToInt(onlinePrice), desc, ConvertToInt(reqId),
                   ConvertToInt(kind), 1, onlineType)).OnlineID.ToString() : FillParameterFailResponse;
        }

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

        private bool ReversalRequest(string onlineId)
        {
            bool result = false;
            string errorCode = string.Empty;

            SelectBankDetailResult SelectBankDetailResult = repository.SelectBankDetail(new SelectBankDetailParameter(onlineId)).SingleOrDefault();

            SelectOnlinePayDetailResult SelectOnlinePayDetailResult = repository.SelectOnlinePayDetail(new SelectOnlinePayDetailParameter(onlineId)).FirstOrDefault();

            if (SelectBankDetailResult is not null || SelectOnlinePayDetailResult is not null)
            {
                if (SelectOnlinePayDetailResult.Online_Status == false)
                {
                    errorCode = mellatGateway.bpReversalRequest(new bpReversalRequest(new bpReversalRequestBody
                         (long.Parse(SelectBankDetailResult.Bank_MerchantID), SelectBankDetailResult.Bank_User, SelectBankDetailResult.Bank_Pass, long.Parse(onlineId), long.Parse(onlineId), long.Parse(SelectOnlinePayDetailResult.Online_OrderNo.ToString()))
                         )).Body.@return;

                    repository.UpdateOnlinePayReversal(new UpdateOnlinePayReversalParameter(onlineId, errorCode));
                    result = errorCode == "0";
                }
            }
            return result;
        }

        public bool RefundRequest(string onlineId, string refundAmount)
        {
            bool result = false;
            string errorCode = string.Empty;

            SelectBankDetailResult SelectBankDetailResult = repository.SelectBankDetail(new SelectBankDetailParameter(onlineId)).SingleOrDefault();

            SelectOnlinePayDetailResult SelectOnlinePayDetailResult = repository.SelectOnlinePayDetail(new SelectOnlinePayDetailParameter(onlineId)).FirstOrDefault();

            if (SelectBankDetailResult is not null || SelectOnlinePayDetailResult is not null)
            {
                if (SelectOnlinePayDetailResult.Online_Status == false)
                {
                    long amount = Helper.RefundAmount(long.Parse(refundAmount), SelectOnlinePayDetailResult);

                    var insertResult = repository.InsertOnlinePay(new InsertOnlinePayParameter
                    (
                        ConvertToInt(SelectBankDetailResult.Bank_ID.ToString()),
                        ConvertToInt(SelectBankDetailResult.Site_ID.ToString()),
                        ConvertToInt(amount.ToString()),
                        onlineId,
                        ConvertToInt(SelectOnlinePayDetailResult.Online_ReqID.ToString()),
                        ConvertToInt(SelectOnlinePayDetailResult.Online_Kind.ToString()),
                        1,
                         "refund")
                    );

                    errorCode = mellatGateway.bpRefundRequest(new bpRefundRequest(new bpRefundRequestBody(
                       long.Parse(SelectBankDetailResult.Bank_MerchantID), SelectBankDetailResult.Bank_User, SelectBankDetailResult.Bank_Pass, long.Parse(insertResult.OnlineID.ToString()), long.Parse(onlineId), long.Parse(SelectOnlinePayDetailResult.Online_OrderNo), amount))).Body.@return;

                    repository.UpdateOnlinePayRefund(new UpdateOnlinePayRefundParameter(insertResult.OnlineID.ToString(), errorCode));

                    result = errorCode == "0";
                }
            }
            return result;
        }

        public bool Cancel(string onlineId)
        {
            var paymentDetail =  repository.SelectPaymentDetail(new SelectPaymentDetailParameter(onlineId));
            if(paymentDetail == null) return false;

            var tranResult = asanRestService.TransactionResult(Convert.ToInt32(paymentGatewaySetting.Value.AsanMerchantId), Convert.ToInt64(localInvoiceId), paymentDetail).Result;

            var cancelResult = asanRestService.CancelTransaction(new CancelCommand(ConvertToInt(paymentDetail.Bank_MerchantID), (ulong)tranResult.PayGateTranID), paymentDetail.Bank_User, paymentDetail.Bank_Pass).Result;

            if (cancelResult.ResCode != 800) throw new Exception(cancelResult.ResMessage);

            //TODO
            
            return true;
        }

        public bool SettleRequest(string onlineId)
        {
            //TODO SiteError

            bool result = false;
            string settleResult = "0";
            string verifyResult = "0";
            string inquieryResult = "0";

            var SelectBankDetailResult = repository.SelectBankDetail(new SelectBankDetailParameter(onlineId)).FirstOrDefault();

            var SelectOnlinePayDetailResult = repository.SelectOnlinePayDetail(new SelectOnlinePayDetailParameter(onlineId)).FirstOrDefault();

            var mellatRequest = new
            {
                merchantID = long.Parse(SelectBankDetailResult.Bank_MerchantID),
                BankUser = SelectBankDetailResult.Bank_User,
                BankPass = SelectBankDetailResult.Bank_Pass,
                OrderID = long.Parse(onlineId),
                SaleOrderID = long.Parse(onlineId),
                OrderNo = long.Parse(SelectOnlinePayDetailResult.Online_OrderNo)
            };

            if (SelectBankDetailResult is not null || SelectOnlinePayDetailResult is not null)
            {
                if (SelectOnlinePayDetailResult.IsSettle != 1)
                {
                    verifyResult = mellatGateway.bpVerifyRequest(new bpVerifyRequest(new bpVerifyRequestBody(
                        mellatRequest.merchantID, mellatRequest.BankUser, mellatRequest.BankPass, mellatRequest.OrderID,
                        mellatRequest.SaleOrderID, mellatRequest.OrderNo))).Body.@return;

                    if (verifyResult != "0")
                    {
                        inquieryResult = mellatGateway.bpInquiryRequest(new bpInquiryRequest(new bpInquiryRequestBody(
                        mellatRequest.merchantID, mellatRequest.BankUser, mellatRequest.BankPass, mellatRequest.OrderID,
                        mellatRequest.SaleOrderID, mellatRequest.OrderNo))).Body.@return;
                    }
                    if (inquieryResult != "0")
                    {
                        if (verifyResult == "0" || inquieryResult == "0")
                            repository.UpdateOnlinePayResWithSettle(new UpdateOnlinePayResWithSettleParameter(ConvertToInt(onlineId)));

                        repository.UpdateOnlinePaySettleFailed(new UpdateOnlinePayResWithSettleFailedParameter(onlineId, ConvertToInt(verifyResult)));
                    }
                    else
                    {
                        settleResult = mellatGateway.bpSettleRequest(new bpSettleRequest(new bpSettleRequestBody(
                             mellatRequest.merchantID, mellatRequest.BankUser, mellatRequest.BankPass, mellatRequest.OrderID,
                             mellatRequest.SaleOrderID, mellatRequest.OrderNo))).Body.@return;
                    }
                    result = true;
                }
            }

            return result;
        }

        #endregion
    }
}
