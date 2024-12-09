using Newtonsoft.Json;
using RestService.models;
using RestService.models.bill;
using RestService.models.reverse;
using RestService.models.settle;
using RestService.models.verify;
using RestService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.StoreProceduresModels.Response;
using Microsoft.AspNetCore.Http;
using BamdadPaymentCore.Domain.Services;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.Common;
using Microsoft.Extensions.Options;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models;
using PGTesterApp.Business;

namespace RestService
{
    public class AsanResetService(IBamdadPaymentRepository repository, IOptions<PaymentGatewaySetting> paymentGatewaySetting) : IAsanRestService
    {
        #region PrivateFields

        private const string REST_URL = "https://ipgrest.asanpardakht.ir/";

        #endregion


        public async Task<ReverseVm> ReverseTransaction(AsanRestRequest requst)
        {
            var client = CreateClient(requst.BankUser, requst.BankPassword);

            var reverseCommand = new ReverseCommand { payGateTranId = requst.payGateTranId, merchantConfigurationId = requst.merchantConfigurationId };

            var content = new StringContent(JsonConvert.SerializeObject(reverseCommand), Encoding.UTF8, "application/json");

            try
            {
                var responseMessage = await client.PostAsync($"v1/Reverse", content,
                                       new CancellationTokenSource(TimeSpan.FromSeconds(20)).Token);
                switch ((int)responseMessage.StatusCode)
                {
                    case 200:
                        return new ReverseVm() { ResCode = 0, ResMessage = "reversing  request succeeded" };
                    case 400:
                        return new ReverseVm() { ResCode = 400, ResMessage = "bad request" };
                    case 401:
                        return new ReverseVm() { ResCode = 401, ResMessage = "unauthorized. probably wrong or unsent header(s)" };
                    case 471:
                        return new ReverseVm() { ResCode = 471, ResMessage = "original transaction was failed" };
                    case 472:
                        return new ReverseVm() { ResCode = 472, ResMessage = "cant reverse a verified transaction" };
                    case 473:
                        return new ReverseVm() { ResCode = 473, ResMessage = "transaction already requested for reversal" };
                    case 474:
                        return new ReverseVm() { ResCode = 474, ResMessage = "transaction already requested for reconcilation" };
                    case 475:
                        return new ReverseVm() { ResCode = 475, ResMessage = "transaction already listed for reversal" };
                    case 476:
                        return new ReverseVm() { ResCode = 476, ResMessage = "transaction already listed for reconcilation" };
                    case 477:
                        return new ReverseVm() { ResCode = 477, ResMessage = "identity not trusted to proceed" };

                    case 571:
                        return new ReverseVm() { ResCode = 571, ResMessage = "not yet processed" };
                    case 572:
                        return new ReverseVm() { ResCode = 572, ResMessage = "transaction status undetermined" };
                    case 573:
                        return new ReverseVm() { ResCode = 573, ResMessage = "unable to request for reversal  due to an internal error" };
                    default:
                        return new ReverseVm() { ResCode = (int)responseMessage.StatusCode, ResMessage = responseMessage.ReasonPhrase };
                }
            }
            catch (TaskCanceledException)
            {
                return new ReverseVm() { ResCode = (int)HttpStatusCode.GatewayTimeout, ResMessage = "Gateway Timeout" };
            }
        }

        public async Task<VerifyVm> VerifyTransaction(AsanRestRequest requst)
        {
            VerifyCommand verifyCommand = new VerifyCommand
            {
                merchantConfigurationId = requst.merchantConfigurationId,
                payGateTranId = requst.payGateTranId
            };
            var client = CreateClient(requst.BankUser, requst.BankPassword);

            var content = new StringContent(JsonConvert.SerializeObject(verifyCommand), Encoding.UTF8, "application/json");

            try
            {
                var responseMessage = (client.PostAsync($"v1/Verify", content, new CancellationTokenSource(TimeSpan.FromSeconds(20)).Token).GetAwaiter().GetResult());
                switch ((int)responseMessage.StatusCode)
                {
                    case 200:
                        return new VerifyVm() { ResCode = 0, ResMessage = "verification succeeded" };
                    case 400:
                        return new VerifyVm() { ResCode = 400, ResMessage = "bad request" };
                    case 401:
                        return new VerifyVm() { ResCode = 401, ResMessage = "unauthorized. probably wrong or unsent header(s)" };
                    case 471:
                        return new VerifyVm() { ResCode = 471, ResMessage = "original transaction was failed" };
                    case 472:
                        return new VerifyVm() { ResCode = 472, ResMessage = "transaction already requested for verification" };
                    case 473:
                        return new VerifyVm() { ResCode = 473, ResMessage = "transaction already requested for reconcilation" };
                    case 474:
                        return new VerifyVm() { ResCode = 474, ResMessage = "transaction already requested for reversal" };
                    case 475:
                        return new VerifyVm() { ResCode = 475, ResMessage = "transaction already listed for reconcilation" };
                    case 476:
                        return new VerifyVm() { ResCode = 476, ResMessage = "transaction already listed for reversal" };
                    case 477:
                        return new VerifyVm() { ResCode = 477, ResMessage = "identity not trusted to proceed" };
                    case 478:
                        return new VerifyVm() { ResCode = 478, ResMessage = "verification already cancelled" };
                    case 571:
                        return new VerifyVm() { ResCode = 571, ResMessage = "not yet processed" };
                    case 572:
                        return new VerifyVm() { ResCode = 572, ResMessage = "transaction status undetermined" };
                    case 573:
                        return new VerifyVm() { ResCode = 573, ResMessage = "unable to request for verification due to an internal error" };
                    default:
                        return new VerifyVm() { ResCode = (int)responseMessage.StatusCode, ResMessage = responseMessage.ReasonPhrase };
                }
            }
            catch (TaskCanceledException)
            {
                return new VerifyVm() { ResCode = (int)HttpStatusCode.GatewayTimeout, ResMessage = "Gateway Timeout" };
            }
        }

        public async Task<SettleVm> SettleTransaction(AsanRestRequest requst)
        {
            var client = CreateClient(requst.BankUser, requst.BankPassword);
            SettleCommand settleCommand = new SettleCommand()
            {
                merchantConfigurationId = requst.merchantConfigurationId,
                payGateTranId = requst.payGateTranId
            };
            var content = new StringContent(JsonConvert.SerializeObject(settleCommand), Encoding.UTF8, "application/json");

            try
            {
                var responseMessage = (client.PostAsync($"v1/Settlement", content, new CancellationTokenSource(TimeSpan.FromSeconds(20)).Token).GetAwaiter().GetResult());
                switch ((int)responseMessage.StatusCode)
                {
                    case 200:
                        return new SettleVm() { ResCode = 0, ResMessage = "settlement request succeeded" };
                    case 400:
                        return new SettleVm() { ResCode = 400, ResMessage = "bad request" };
                    case 401:
                        return new SettleVm() { ResCode = 401, ResMessage = "unauthorized. probably wrong or unsent header(s)" };
                    case 471:
                        return new SettleVm() { ResCode = 471, ResMessage = "original transaction was failed" };
                    case 472:
                        return new SettleVm() { ResCode = 472, ResMessage = "transaction not verified" };
                    case 473:
                        return new SettleVm() { ResCode = 473, ResMessage = "transaction already requested for reversal" };
                    case 474:
                        return new SettleVm() { ResCode = 474, ResMessage = "transaction already requested for reconcilation" };
                    case 475:
                        return new SettleVm() { ResCode = 475, ResMessage = "transaction already listed for reversal" };
                    case 476:
                        return new SettleVm() { ResCode = 476, ResMessage = "transaction already listed for reconcilation" };
                    case 477:
                        return new SettleVm() { ResCode = 477, ResMessage = "identity not trusted to proceed" };
                    case 478:
                        return new SettleVm() { ResCode = 478, ResMessage = "verification already cancelled" };
                    case 571:
                        return new SettleVm() { ResCode = 571, ResMessage = "not yet processed" };
                    case 572:
                        return new SettleVm() { ResCode = 572, ResMessage = "transaction status undetermined" };
                    case 573:
                        return new SettleVm() { ResCode = 573, ResMessage = "unable to request for reconcilation due to an internal error" };
                    default:
                        return new SettleVm() { ResCode = (int)responseMessage.StatusCode, ResMessage = responseMessage.ReasonPhrase };
                }
            }
            catch (TaskCanceledException)
            {
                return new SettleVm() { ResCode = (int)HttpStatusCode.GatewayTimeout, ResMessage = "Gateway Timeout" };
            }
        }

        public async Task<PaymentResultVm> TransactionResult(TransactionResultRequest request)
        {
            var client = CreateClient(request.bankUser, request.bankPassword);

            PaymentResultVm paymentResultVm;
            try
            {
                var responseMessage = client.GetAsync($"v1/TranResult?LocalInvoiceId={request.localInvoiceId}" +
                                                             $"&MerchantConfigurationId={request.merchantConfigId}",
                                                              new CancellationTokenSource(TimeSpan.FromSeconds(20)).Token).GetAwaiter().GetResult();
                switch ((int)responseMessage.StatusCode)
                {
                    case 200:
                        var content = await responseMessage.Content.ReadAsStringAsync();

                        paymentResultVm = JsonConvert.DeserializeObject<PaymentResultVm>(content);
                        paymentResultVm.ResCode = 0;
                        paymentResultVm.ResMessage = responseMessage.ReasonPhrase;
                        return paymentResultVm;
                    case 472:
                        return new PaymentResultVm() { ResCode = 472, ResMessage = "no records found" };
                    case 408:
                        return new PaymentResultVm() { ResCode = 471, ResMessage = "identity not trusted to proceed" };
                    case 504:
                        return new PaymentResultVm() { ResCode = 401, ResMessage = "unauthorized. probably wrong or unsent header(s)" };

                    case 571:
                        return new PaymentResultVm() { ResCode = 571, ResMessage = "error in processing" };
                    default:
                        return new PaymentResultVm() { ResCode = (short)responseMessage.StatusCode, ResMessage = responseMessage.ReasonPhrase };
                }
            }
            catch (TaskCanceledException)
            {
                return new PaymentResultVm() { ResCode = 408, ResMessage = "Timeout By WebPay Application" };
            }
            catch (Exception ex)
            {
                return new PaymentResultVm() { ResCode = 500, ResMessage = ex.Message };
            }
        }

        public async Task<CancelResultVm> CancelTransaction(AsanRestRequest requst)
        {
            var client = CreateClient(requst.BankUser, requst.BankPassword);
            CancelCommand cancelCommand = new CancelCommand()
            {
                merchantConfigurationId = requst.merchantConfigurationId,
                payGateTranId = requst.payGateTranId
            };
            var content = new StringContent(JsonConvert.SerializeObject(cancelCommand), Encoding.UTF8, "application/json");
            try
            {
                var responseMessage = await client.PostAsync($"v1/Cancel", content,
                                       new CancellationTokenSource(TimeSpan.FromSeconds(20)).Token);
                switch ((int)responseMessage.StatusCode)
                {
                    case 200:
                        return new CancelResultVm() { ResCode = 200, ResMessage = "transaction review cancellation request succeeded" };
                    case 801:
                        return new CancelResultVm() { ResCode = 801, ResMessage = "processing not yet completed" };
                    case 802:
                        return new CancelResultVm() { ResCode = 802, ResMessage = "transaction status undetermined" };
                    case 803:
                        return new CancelResultVm() { ResCode = 803, ResMessage = "original transaction was failed" };
                    case 804:
                        return new CancelResultVm() { ResCode = 804, ResMessage = "transaction not yet reviewed" };
                    case 805:
                        return new CancelResultVm() { ResCode = 805, ResMessage = "settlement request already made for this transaction" };
                    case 806:
                        return new CancelResultVm() { ResCode = 806, ResMessage = "reversal request already made for this transaction" };
                    case 807:
                        return new CancelResultVm() { ResCode = 807, ResMessage = "transaction listed under pending settlements" };
                    case 808:
                        return new CancelResultVm() { ResCode = 808, ResMessage = "transaction listed under pending reversals" };
                    case 809:
                        return new CancelResultVm() { ResCode = 809, ResMessage = "operation cannot be performed due to an internal issue" };
                    case 810:
                        return new CancelResultVm() { ResCode = 810, ResMessage = "identity of the requester is invalid" };
                    case 811:
                        return new CancelResultVm() { ResCode = 811, ResMessage = "this action is not allowed for bill payment transactions" };
                    case 812:
                        return new CancelResultVm() { ResCode = 812, ResMessage = "review details not found" };
                    case 813:
                        return new CancelResultVm() { ResCode = 813, ResMessage = "review has already been canceled" };
                    case 814:
                        return new CancelResultVm() { ResCode = 814, ResMessage = "cannot request after the allowable time post-verification" };
                    default:
                        return new CancelResultVm() { ResCode = (int)responseMessage.StatusCode, ResMessage = responseMessage.ReasonPhrase };

                }
            }
            catch (TaskCanceledException)
            {
                return new CancelResultVm() { ResCode = (int)HttpStatusCode.GatewayTimeout, ResMessage = "Gateway Timeout" };
            }
        }

        public async Task<TResult> GetToken<TRequest, TResult>(TRequest request, SelectPaymentDetailResult paymentDetail) where TRequest : ITokenCommand where TResult : class, ITokenVm, new()
        {
            var refid = string.Empty;

            var client = CreateClient(paymentDetail.Bank_User, paymentDetail.Bank_Pass);

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            try
            {
                var responseMessage = client.PostAsync($"v1/Token", content, new CancellationTokenSource(TimeSpan.FromSeconds(20)).Token).GetAwaiter().GetResult();
                switch ((int)responseMessage.StatusCode)
                {
                    case 200:
                        refid = await responseMessage.Content.ReadAsStringAsync();
                        return new TResult { RefId = JsonConvert.DeserializeObject<string>(refid), ResCode = 0 };
                    case 489:
                        return new TResult { ResCode = 489, ResMessage = "duplicate local invoice id" };
                    case 484:
                        return new TResult { ResCode = 484, ResMessage = "internal error for other reasons" };
                    case 486:
                        return new TResult { ResCode = 486, ResMessage = "amount is not in range" };
                    case 504:
                        return new TResult { ResCode = 504, ResMessage = responseMessage.ReasonPhrase };
                    default:
                        return new TResult { ResCode = (int)responseMessage.StatusCode, ResMessage = "unknown" };
                }
            }
            catch (TaskCanceledException)
            {
                return new TResult { ResCode = (int)HttpStatusCode.GatewayTimeout, ResMessage = "Gateway Timeout" };
            }
            catch (Exception ex)
            {
                return new TResult { ResCode = (int)HttpStatusCode.GatewayTimeout, ResMessage = ex.Message };
            }
        }

        public string Return(HttpRequest Request)
          => string.IsNullOrEmpty(Request.Form["PaygateTranId"])
          ? CancelPayment(Request.Query["invoiceid"])
          : ProcessAsanPardakhtPayment(Request.Query["invoiceid"]);

        public AsanTransactionResult GetTransationResultFromAsanPardakht(string onlineId, SelectPaymentDetailResult paymentDetail)
        {
            var tranReq = new TransactionResultRequest
            (
                paymentGatewaySetting.Value.AsanMerchantId,
                onlineId,
                paymentDetail.Bank_User,
                paymentDetail.Bank_Pass
            );

            var tranResult = TransactionResult(tranReq).Result;

            string saleOrderId = onlineId;
            string refId = tranResult.RefId;
            string saleReferenceId = tranResult.PayGateTranID.ToString();
            string cardHolderInfo = tranResult.CardNumber;
            string referenceNumber = tranResult.Rrn;

            repository.InsertTransactionResult
                (new InsertTransactionResultParameter(referenceNumber, Convert.ToInt32(saleOrderId), refId, saleReferenceId, tranResult.ResCode, cardHolderInfo));

            return new(refId, saleReferenceId, cardHolderInfo, referenceNumber, tranResult.ResMessage, tranResult.ResCode);
        }

        public SettleVm SettleAsan(AsanTransactionResult tranResult, SelectPaymentDetailResult paymentDetail, string onlineId)
        {
            var settleCommand = new AsanRestRequest
            {
                merchantConfigurationId = int.Parse(paymentGatewaySetting.Value.AsanMerchantId),
                payGateTranId = Convert.ToUInt64(tranResult.saleReferenceId),
                BankUser = paymentDetail.Bank_User,
                BankPassword = paymentDetail.Bank_Pass,
            };

            var settleRes = SettleTransaction(settleCommand).Result;

            if (settleRes.ResCode != 0)
            {
                repository.UpdateOnlinePaySettleFailed(new UpdateOnlinePayResWithSettleFailedParameter(onlineId, settleRes.ResCode));
            }

            repository.UpdateOnlinePayResWithSettle(
              new UpdateOnlinePayResWithSettleParameter(ConvertToInt(onlineId)));

            return settleRes;
        }

        public string SendToAsanPardakhtPaymentGateway(SelectPaymentDetailResult paymentDetail, string onlineId)
        {
            var paymentToken = new RequestCommand
            (
             Convert.ToInt32(paymentDetail.Bank_MerchantID.ToString()),
             Convert.ToInt32(ServiceTypeEnum.Sale),
             Convert.ToInt64(onlineId),
             Convert.ToUInt64(paymentDetail.Online_Price.ToString()),
              $"{paymentGatewaySetting.Value.MelatReturnBank}?invoiceID={onlineId}",
             "پرداخت"
            );

            var tokenResult = GetToken<RequestCommand, RequestTokenVm>(paymentToken, paymentDetail).Result;

            if (tokenResult.ResCode == 0)
                return tokenResult.RefId;


            return tokenResult.ResMessage + string.Format(" ({0})", tokenResult.ResCode);
        }

        public bool Cancel(string onlineId)
        {
            var paymentDetail = repository.SelectPaymentDetail(new SelectPaymentDetailParameter(onlineId));
            if (paymentDetail == null || paymentDetail.Online_Status == false || paymentDetail.Online_Price == 0) throw new PaymentDetailException();

            var tranResult = GetTransationResultFromAsanPardakht(onlineId, paymentDetail);

            if (tranResult.resCode != 0) throw new GetTransationResultException();

            var CancelCommand = new AsanRestRequest()
            {
                merchantConfigurationId = ConvertToInt(paymentDetail.Bank_MerchantID),
                payGateTranId = ulong.Parse(tranResult.saleReferenceId),
                BankUser = paymentDetail.Bank_User,
                BankPassword = paymentDetail.Bank_Pass,
            };

            var cancelResult = CancelTransaction(CancelCommand).Result;

            if (cancelResult.ResCode != 200) throw new CancelTransationException();

            repository.UpdateOnlinePayRefund(new UpdateOnlinePayRefundParameter(ConvertToInt(onlineId), cancelResult.ResCode));

            return true;
        }

        #region PrivateMethods

        private HttpClient CreateClient(string bankUser, string bankPass)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(REST_URL),
                Timeout = TimeSpan.FromSeconds(20)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("usr", bankUser);
            client.DefaultRequestHeaders.Add("pwd", bankPass);

            return client;
        }

        private string CancelPayment(string onlineId)
             => repository.UpdateOnlinePaymentFailed(new UpdateOnlinePayFailedParameter(null, Convert.ToInt32(onlineId),
                     "cancel", "Failed", -1, "use cancel payment"))
                 .Site_ReturnUrl;

        public string ProcessAsanPardakhtPayment(string onlineId)
        {
            string localInvoiceId = onlineId;

            if (string.IsNullOrEmpty(localInvoiceId)) return SiteErrorResponse.NullOrEmptyOnlineId;

            var paymentDetail = repository.SelectPaymentDetail(new SelectPaymentDetailParameter(localInvoiceId));

            if (paymentDetail == null) return SiteErrorResponse.PaymentNotValid;

            //Free Payment
            if (paymentDetail.Online_Price == 0) return FreePayment(localInvoiceId);

            var tranResult = GetTransationResultFromAsanPardakht(onlineId, paymentDetail);

            if (tranResult.resCode == 911) return CancelPayment(onlineId);

            if (tranResult.resCode != 0)
                return UpdateOnlinePayFailed(tranResult.referenceNumber, onlineId, tranResult.refId, tranResult.saleReferenceId,
                    tranResult.resCode.ToString(), tranResult.cardHolderInfo);

            var verifyCommand = new AsanRestRequest
            {
                merchantConfigurationId = Convert.ToInt32(paymentDetail.Bank_MerchantID.ToString()),
                payGateTranId = Convert.ToUInt64(tranResult.saleReferenceId),
                BankUser = paymentDetail.Bank_User,
                BankPassword = paymentDetail.Bank_Pass,
            };
            var verifyRes = VerifyTransaction(verifyCommand).Result;

            if (verifyRes.ResCode != 0)
            {
                return repository.UpdateOnlinePaymentFailed(new UpdateOnlinePayFailedParameter(tranResult.referenceNumber,
                     ConvertToInt(onlineId), tranResult.refId, tranResult.saleReferenceId.ToString(), verifyRes.ResCode,
                     tranResult.cardHolderInfo)).Site_ReturnUrl;
            }

            var url = repository.UpdateOnlinePayment(new UpdateOnlinePayParameter(tranResult.referenceNumber,
                ConvertToInt(onlineId), tranResult.refId, tranResult.saleReferenceId, verifyRes.ResCode, tranResult.cardHolderInfo)).Site_ReturnUrl;

            if (paymentDetail.AutoSettle is false) return url;

            var settleRes = SettleAsan(tranResult, paymentDetail, onlineId);

            var updateResult = repository.UpdateOnlinePayment(new UpdateOnlinePayParameter(tranResult.referenceNumber,
              ConvertToInt(onlineId), tranResult.refId, tranResult.saleReferenceId, settleRes.ResCode, tranResult.cardHolderInfo));

            if (updateResult.Success == 1) return updateResult.Site_ReturnUrl;

            return SiteErrorResponse.NullOrEmptyOnlineId;
        }

        public string UpdateOnlinePayFailed(string referenceNumber, string onlineId, string transactionNo,
            string orderNo, string errorCode, string cardHolderInfo)
            => repository.UpdateOnlinePaymentFailed(new UpdateOnlinePayFailedParameter(referenceNumber,
                    ConvertToInt(onlineId), transactionNo, orderNo, ConvertToInt(errorCode), cardHolderInfo))
                .Site_ReturnUrl;

        private int ConvertToInt(string value) => Convert.ToInt32(value);

        private string FreePayment(string onlineId)
           => repository
               .UpdateOnlinePayment(new UpdateOnlinePayParameter("", ConvertToInt(onlineId), "Free", "Free", -1, ""))
               .Site_ReturnUrl;
        #endregion
    }
}