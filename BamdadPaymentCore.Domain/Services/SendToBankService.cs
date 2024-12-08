using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.Enums;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.StoreProceduresModels.Response;
using bpm.shaparak.ir;
using Microsoft.Extensions.Options;
using PGTesterApp.Business;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Services
{
    public class SendToBankService(IPaymentService paymentService, IOptions<PaymentGatewaySetting> paymentGatewaySetting, IBankService asanRestService
        , IPaymentGateway mellatPaymentGateway, IBamdadPaymentRepository paymentRepository) : ISendToBankService
    {

        public string SendToBank(string onlineId)
        {
            SelectPaymentDetailResult paymentDetail = null;

            if (!string.IsNullOrWhiteSpace(onlineId)) paymentDetail = paymentRepository.SelectPaymentDetail(new SelectPaymentDetailParameter(onlineId));

            if (paymentDetail is null || paymentDetail.Online_Status == true) return SiteErrorResponse.PaymentNotValid;

            //Return Redirection URL To Already Saved Sites
            if (paymentDetail.Online_Price == 0) return paymentService.FreePayment(onlineId);

            if (paymentDetail.BankCode == nameof(BankCode.Mellat)) return SendToMellatPaymentGateway(paymentDetail, onlineId);

            if (paymentDetail.BankCode == nameof(BankCode.Parsian)) return SendToPasianPaymentGateway(paymentDetail, onlineId);

            if (paymentDetail.BankCode == nameof(BankCode.Asan)) return SendToAsanPardakhtPaymentGateway(paymentDetail, onlineId);

            return SiteErrorResponse.NullOrEmptyOnlineId;
        }

        public string SendToMellatPaymentGateway(SelectPaymentDetailResult paymentDetail, string onlineId)
        {
            DateTime now = DateTime.Now;
            string time = now.ToString("HHmmss");
            string date = now.ToString("yyyyMMdd");

            string mellatResponse = paymentDetail.Online_Kind == 1 || paymentDetail.Online_Kind == 0
             ? PayNormalMelat(paymentDetail, onlineId, date, time, paymentGatewaySetting.Value.MelatReturnBankWithAccept, paymentGatewaySetting.Value.MelatReturnBank)
             : PayWithKind(paymentDetail, onlineId, date, time, paymentGatewaySetting.Value.MelatReturnBankWithAccept, paymentGatewaySetting.Value.MelatReturnBank);


            if (string.IsNullOrEmpty(mellatResponse)) return SiteErrorResponse.PaymentNotValid;

            string[] strArray = mellatResponse.Split(',');

            return strArray[0] == "0"
                ? $"<script language='javascript' type='text/javascript'> postRefId('" + strArray[1] + "');</script> "
                : SiteErrorResponse.BankConnectionFailed + strArray[0];
        }

        public string SendToPasianPaymentGateway(SelectPaymentDetailResult paymentDetail, string onlineId)
        {
            throw new NotImplementedException();
        }

        public string SendToAsanPardakhtPaymentGateway(SelectPaymentDetailResult paymentDetail, string onlineId)
        {
            var paymentToken = new RequestCommand(Convert.ToInt32(paymentDetail.Bank_MerchantID.ToString()),
                                           Convert.ToInt32(ServiceTypeEnum.Sale),
                                           Convert.ToInt64(onlineId),
                                           Convert.ToUInt64(paymentDetail.Online_Price.ToString()),
                                            $"{paymentGatewaySetting.Value.MelatReturnBank}?invoiceID={onlineId}",
                                           "پرداخت"
                                          );



            var tokenResult = asanRestService.GetToken<RequestCommand, RequestTokenVm>(paymentToken, paymentDetail).Result;

            if (tokenResult.ResCode == 0)
                return RedirectWithPost.PreparePostForm(paymentGatewaySetting.Value.AsanpardakhtGateWay, new Dictionary<string, string> { { "RefId", tokenResult.RefId } });

            return tokenResult.ResMessage + string.Format(" ({0})", tokenResult.ResCode);
        }


        #region PrivateMethods

        private string PayNormalMelat(SelectPaymentDetailResult paymentDetail, string reqOnlineId, string date, string time, string ReturnBankWithAccept, string ReturnBank)
        {
            var t = mellatPaymentGateway.bpPayRequest(new bpPayRequest(new bpPayRequestBody(
                long.Parse(paymentDetail.Bank_MerchantID.ToString())
                , paymentDetail.Bank_User
                , paymentDetail.Bank_Pass
                , long.Parse(reqOnlineId)
                , Convert.ToInt64(paymentDetail.Online_Price)
                , date
                , time
                , "0"
                , paymentDetail.IsSettle == 1 ? ReturnBankWithAccept : ReturnBank
                , "0"
                , ""
                , ""
                , ""
                , ""
                , null
                ))).Body.@return;
            return t;
        }

        private string PayWithKind(SelectPaymentDetailResult paymentDetail, string reqOnlineId, string date, string time, string ReturnBankWithAccept, string ReturnBank)
        {
            return mellatPaymentGateway.bpDynamicPayRequest(new bpDynamicPayRequest(new bpDynamicPayRequestBody(
                long.Parse(paymentDetail.Bank_MerchantID.ToString())
                , paymentDetail.Bank_User.ToString()
                , paymentDetail.Bank_Pass.ToString()
                , long.Parse(reqOnlineId)
                , long.Parse(paymentDetail.Online_Price.ToString())
                , date
                , time
                , "0"
                , paymentDetail.IsSettle == 1 ? ReturnBankWithAccept : ReturnBank
                , "0"
                , long.Parse(paymentDetail.Online_Kind.ToString())
                , ""
                , ""
                , ""
                , ""
                , null))).Body.@return;
        }

        #endregion
    }
}
