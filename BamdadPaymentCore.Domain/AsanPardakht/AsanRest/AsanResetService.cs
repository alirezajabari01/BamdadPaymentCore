using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.Common;
using Microsoft.Extensions.Options;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.settlement;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.verify;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.bill;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.reverse;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.Models.ControllerDto;
using BamdadPaymentCore.Domain.Enums;
using Uri = System.Uri;

namespace BamdadPaymentCore.Domain.AsanPardakht.AsanRest
{
    public class AsanResetService(IBamdadPaymentRepository repository, IOptionsSnapshot<PaymentGatewaySetting> paymentGatewaySetting) : IAsanRestService
    {

        public async Task PayFailedInSettlePayments()
        {
            var faileds = await repository.GetFailedInSettleBankPayments(new GetFailedVerifyPaymentsParameter(AsanError.SettleErrored));

            foreach (var faile in faileds)
            {
                try
                {
                    ProcessPayFailedSettlePayments(faile);
                }
                catch (Exception ex)
                {
                    repository.insertSiteError(new InsertSiteErrorParameter(ex.Message, "back groundjob PayFailedInSettlePayments thrown Exception"));
                }
            }
        }

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
                var responseMessage = client.PostAsync($"v1/Verify", content, new CancellationTokenSource(TimeSpan.FromSeconds(20)).Token).GetAwaiter().GetResult();
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
                var responseMessage = client.PostAsync($"v1/Settlement", content, new CancellationTokenSource(TimeSpan.FromSeconds(20)).Token).GetAwaiter().GetResult();
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

            return new(tranResult.RefId, tranResult.PayGateTranID.ToString(), tranResult.CardNumber, tranResult.Rrn, tranResult.ResMessage, tranResult.ResCode);
        }

        public SettleVm SettleAsan(SettleAsanInput input)
        {
            var settleCommand = new AsanRestRequest
            {
                merchantConfigurationId = int.Parse(paymentGatewaySetting.Value.AsanMerchantId),
                payGateTranId = Convert.ToUInt64(input.SaleReferenceId),
                BankUser = input.bankUser,
                BankPassword = input.BankPass,
            };

            var settleRes = SettleTransaction(settleCommand).Result;

            if (settleRes.ResCode != 0)
            {
                repository.UpdateOnlinePaySettleFailed(new UpdateOnlinePayResWithSettleFailedParameter(input.OnlineId, settleRes.ResCode));
            }
            else
            {
                repository.UpdateOnlinePayResWithSettle(new UpdateOnlinePayResWithSettleParameter(input.OnlineId));

            }

            return settleRes;
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
                repository.UpdateOnlinePaySettleFailed(new UpdateOnlinePayResWithSettleFailedParameter(Int(onlineId), settleRes.ResCode));
            }
            else
            {
                repository.UpdateOnlinePayResWithSettle(new UpdateOnlinePayResWithSettleParameter(Int(onlineId)));
            }
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
              $"{paymentGatewaySetting.Value.AsanCallBackUrl}?invoiceID={onlineId}",
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
                merchantConfigurationId = Int(paymentDetail.Bank_MerchantID),
                payGateTranId = ulong.Parse(tranResult.saleReferenceId),
                BankUser = paymentDetail.Bank_User,
                BankPassword = paymentDetail.Bank_Pass,
            };

            var cancelResult = CancelTransaction(CancelCommand).Result;

            if (cancelResult.ResCode != 200) throw new CancelTransationException();

            repository.UpdateOnlinePayRefund(new UpdateOnlinePayRefundParameter(Int(onlineId), cancelResult.ResCode));

            return true;
        }

        public async Task PayFailedVerifyPayments()
        {
            var pendings = await repository.GetFailedVerifyPayments(new GetFailedVerifyPaymentsParameter(AsanError.VerifyErrored));

            if (pendings != null && pendings.Count > 0)
            {
                foreach (var pending in pendings)
                {
                    try
                    {
                        ProcessPayFailedVerifyPayments(pending);
                    }
                    catch (Exception ex) { repository.insertSiteError(new InsertSiteErrorParameter(ex.Message, "back groundjob PayFailedVerifyPayments thrown Exception")); }

                }
            }
        }

        public string ProcessAsanPardakhtPayment(string onlineId)
        {
            int OnlineIdInt = Int(onlineId);

            if (string.IsNullOrEmpty(onlineId)) return SiteErrorResponse.NullOrEmptyOnlineId;

            SelectPaymentDetailResult paymentDetail = repository.SelectPaymentDetail(new SelectPaymentDetailParameter(onlineId));

            if (paymentDetail is null || paymentDetail.IsSettle is 1) return SiteErrorResponse.PaymentNotValid;

            AsanTransactionResult? tranResult = null;
            string url = string.Empty;
            try
            {
                tranResult = GetTransationResultFromAsanPardakht(onlineId, paymentDetail);
                url = repository.UpdateTransactionResult(new UpdateTransactionResultParameter(tranResult.referenceNumber, onlineId, tranResult.resCode.ToString(), tranResult.refId, tranResult.saleReferenceId, tranResult.cardHolderInfo)).Site_ReturnUrl;
                //repository.Update(new OnlinePay()
                //{
                //    OnlineId = OnlineIdInt,
                //    OnlineErrorCode = tranResult.resCode.ToString(),
                //    AutoSettle = paymentDetail.AutoSettle,
                //    CardHolderInfo = tranResult.cardHolderInfo ?? "",
                //    IsSettle = paymentDetail.IsSettle,
                //    OnlineOrderNo = tranResult.saleReferenceId ?? "",
                //    OnlineTransactionNo = tranResult.refId ?? "",
                //    ReferenceNumber = tranResult.referenceNumber ?? "",
                //    OnlinePrice = paymentDetail.Online_Price
                //});

                if (tranResult.resCode != 0)
                {
                    return InsertTransactionResultError(paymentDetail, tranResult, onlineId);
                }
                else
                {
                    url = UpdateTransactionResult(tranResult, onlineId);

                }
            }
            catch (Exception ex)
            {
                LogInsertSiteError(ex.Message, SiteErrorResponse.BankTransationResultException);
                return InsertTransactionResultError(paymentDetail, tranResult, onlineId);
            }

            var verifyCommand = new AsanRestRequest
            {
                merchantConfigurationId = Convert.ToInt32(paymentDetail.Bank_MerchantID.ToString()),
                payGateTranId = Convert.ToUInt64(tranResult.saleReferenceId),
                BankUser = paymentDetail.Bank_User,
                BankPassword = paymentDetail.Bank_Pass,
            };
            var verifyRes = Verify(verifyCommand, OnlineIdInt);

            if (verifyRes != "success") return verifyRes;

            if (paymentDetail.AutoSettle is false) return url;

            var input = new SettleAsanInput(paymentDetail.Bank_User, paymentDetail.Bank_Pass, tranResult.saleReferenceId, OnlineIdInt);

            return Settle(input, url, OnlineIdInt);
        }

        public string ProcessPayFailedVerifyPayments(GetFailedVerifyPaymentsResult faileds)
        {
            var verifyCommand = new AsanRestRequest
            {
                merchantConfigurationId = Convert.ToInt32(faileds.Bank_MerchantID),
                payGateTranId = Convert.ToUInt64(faileds.Online_OrderNo),
                BankUser = faileds.Bank_User,
                BankPassword = faileds.Bank_Pass,
            };
            var verifyRes = Verify(verifyCommand, faileds.Online_ID);

            if (verifyRes != "success") return verifyRes;

            repository.UpdateTransactionResult(new UpdateTransactionResultParameter(faileds.ReferenceNumber, faileds.Online_ID.ToString(), 0.ToString(), faileds.Online_TransactionNo, faileds.Online_OrderNo, faileds.CardHolderInfo));

            var input = new SettleAsanInput(faileds.Bank_User, faileds.Bank_Pass, faileds.Online_OrderNo, faileds.Online_ID);

            return Settle(input, faileds.Site_ReturnUrl, faileds.Online_ID);
        }


        #region PrivateMethods

        private string Settle(SettleAsanInput input, string url, int onlineId)
        {
            try
            {
                var settleRes = SettleAsan(input);
                if (settleRes.ResCode == 0) return url;
                return SiteErrorResponse.NullOrEmptyOnlineId;
            }
            catch (Exception ex)
            {
                LogInsertSiteError(ex.Message, SiteErrorResponse.BankSettleException);
                return UpdateIsSettle(onlineId.ToString(), AsanError.SettleErrored);
            }
        }

        private string Verify(AsanRestRequest req, int onlineId)
        {
            try
            {
                var verifyRes = VerifyTransaction(req).Result;

                if (verifyRes.ResCode != 0) return UpdateVerifyFailedPayment(verifyRes.ResCode, onlineId);

                return "success";
            }
            catch (Exception ex)
            {
                LogInsertSiteError(ex.Message, SiteErrorResponse.BankVerifyException);
                return UpdateVerifyFailedPayment(AsanError.VerifyErrored, onlineId);
            }
        }

        private HttpClient CreateClient(string bankUser, string bankPass)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(paymentGatewaySetting.Value.AsanRestURL),
                Timeout = TimeSpan.FromSeconds(20)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("usr", bankUser);
            client.DefaultRequestHeaders.Add("pwd", bankPass);

            return client;
        }

        public string UpdateOnlinePayFailed(string referenceNumber, string onlineId, string transactionNo,
            string orderNo, string errorCode, string cardHolderInfo)
            => repository.UpdateOnlinePaymentFailed(new UpdateOnlinePayFailedParameter(referenceNumber,
                    onlineId, transactionNo, orderNo, Int(errorCode), cardHolderInfo))
                .Site_ReturnUrl;

        public string UpdateTransactionResult(AsanTransactionResult result, string onlineId)
        => repository.UpdateTransactionResult(new UpdateTransactionResultParameter(result.referenceNumber, onlineId, result.refId, result.saleReferenceId, result.cardHolderInfo)).Site_ReturnUrl;

        private int Int(string value) => Convert.ToInt32(value);

        private string InsertTransactionResultError(SelectPaymentDetailResult detail, AsanTransactionResult tranRes, string onlineId)
            => repository.InsertTransactionResultError(new InsertTransactionResultErrorParameter
                (tranRes.resCode, tranRes.resMessage, Convert.ToInt32(onlineId), detail.Bank_MerchantID, detail.Bank_User, detail.Bank_Pass, detail.BankCode, detail.Online_Status, detail.Online_Kind, detail.IsSettle, detail.AutoSettle, detail.MobileNomber)).Site_ReturnUrl;

        private string UpdateOnlinePaymentFailed(AsanTransactionResult tranResult, string onlineId, int resCode)
            => repository.UpdateOnlinePaymentFailed(new UpdateOnlinePayFailedParameter(tranResult.referenceNumber,
                         onlineId, tranResult.refId, tranResult.saleReferenceId.ToString(), resCode, tranResult.cardHolderInfo)).Site_ReturnUrl;

        private string UpdateIsSettle(string onlineId, int error)
            => repository.UpdateIsSettle(new UpdateIsSettleParameter(onlineId, error)).Site_ReturnUrl;

        private void LogInsertSiteError(string message, string reason)
            => repository.insertSiteError(new InsertSiteErrorParameter(message, reason));

        private string UpdateVerifyFailedPayment(int errorCode, int onlineId)
            => repository.UpdateVerifyFailedPayment(new UpdateVerifyFailedPaymentParameter(errorCode.ToString(), onlineId)).Site_ReturnUrl;

        private void ProcessPayFailedSettlePayments(GetFailedSettlePaymentsResult result)
        {
            var input = new SettleAsanInput(result.Bank_User, result.Bank_Pass, result.Online_OrderNo, result.Online_ID);

            Settle(input, result.Site_ReturnUrl, result.Online_ID);
        }

        #endregion
    }
}