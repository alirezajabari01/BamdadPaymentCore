using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.ControllerDto;
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
    public class SendToBankService(IPaymentService paymentService, IOptions<PaymentGatewaySetting> paymentGatewaySetting, IAsanRestService asanRestService
        , IPaymentGateway mellatPaymentGateway, IBamdadPaymentRepository paymentRepository) : ISendToBankService
    {

        public SendToBankResultVm SendToBank(string onlineId)
        {
            var res = new SendToBankResultVm();
            SelectPaymentDetailResult paymentDetail = null;

            if (!string.IsNullOrWhiteSpace(onlineId)) paymentDetail = paymentRepository.SelectPaymentDetail(new SelectPaymentDetailParameter(onlineId));

            if (paymentDetail is null || paymentDetail.Online_Status == true) return res;

            if (paymentDetail.Online_Price == 0) return new SendToBankResultVm(Url: paymentService.FreePayment(onlineId), onlineId);

            if (paymentDetail.BankCode == nameof(BankCode.Mellat))
                return new SendToBankResultVm(paymentGatewaySetting.Value.MellatGateWay, SendToMellatPaymentGateway(paymentDetail, onlineId));

            if (paymentDetail.BankCode == nameof(BankCode.Parsian)) return res;

            if (paymentDetail.BankCode == nameof(BankCode.Asan))
                return new SendToBankResultVm(paymentGatewaySetting.Value.AsanpardakhtGateWay, asanRestService.SendToAsanPardakhtPaymentGateway(paymentDetail, onlineId));

            return res;
        }

        public string SendToMellatPaymentGateway(SelectPaymentDetailResult paymentDetail, string onlineId)
        {
            DateTime now = DateTime.Now;
            string time = now.ToString("HHmmss");
            string date = now.ToString("yyyyMMdd");

            string mellatResponse = paymentDetail.Online_Kind == 1 || paymentDetail.Online_Kind == 0
             ? PayNormalMelat(paymentDetail, onlineId, date, time, paymentGatewaySetting.Value.MelatReturnBankWithAccept, paymentGatewaySetting.Value.MelatReturnBank)
             : PayWithKind(paymentDetail, onlineId, date, time, paymentGatewaySetting.Value.MelatReturnBankWithAccept, paymentGatewaySetting.Value.MelatReturnBank);

            string[] strArray = mellatResponse.Split(',');

            if (string.IsNullOrEmpty(mellatResponse) || strArray[0] != "0") return SiteErrorResponse.BankConnectionFailed;

            return strArray[1];
            //return $"<script language='javascript' type='text/javascript'> postRefId('" + strArray[1] + "');</script> ";
        }

        public string SendToPasianPaymentGateway(SelectPaymentDetailResult paymentDetail, string onlineId)
        {

            throw new NotImplementedException();
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
