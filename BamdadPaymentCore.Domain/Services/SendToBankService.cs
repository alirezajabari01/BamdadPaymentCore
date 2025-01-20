using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.Enums;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.Models.ControllerDto;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;

using bpm.shaparak.ir;
using Microsoft.Extensions.Options;


namespace BamdadPaymentCore.Domain.Services
{
    public class SendToBankService(IPaymentService paymentService, IOptionsSnapshot<PaymentGatewaySetting> paymentGatewaySetting, IAsanRestService asanRestService
        , IBamdadPaymentRepository paymentRepository, IMellatService mellatService) : ISendToBankService
    {

        public SendToBankResultVm SendToBank(string onlineId)
        {
            SelectPaymentDetailResult paymentDetail = null;

            if (!string.IsNullOrWhiteSpace(onlineId)) paymentDetail = paymentRepository.SelectPaymentDetail(new SelectPaymentDetailParameter(onlineId));

            if (paymentDetail is null || paymentDetail.Online_Status == true) return new SendToBankResultVm(Message:"قبلا به بانک ارسال شده");

            if (paymentDetail.Online_Price == 0) return new SendToBankResultVm(Url: paymentService.FreePayment(onlineId), onlineId);

            if (paymentDetail.BankCode == nameof(BankCode.Mellat))
                return new SendToBankResultVm(paymentGatewaySetting.Value.MellatGateWay, mellatService.SendToMellatPaymentGateway(paymentDetail, onlineId));

            if (paymentDetail.BankCode == nameof(BankCode.Parsian)) return null;

            if (paymentDetail.BankCode == nameof(BankCode.Asan))
                return new SendToBankResultVm(paymentGatewaySetting.Value.AsanpardakhtGateWay, asanRestService.SendToAsanPardakhtPaymentGateway(paymentDetail, onlineId));

            return new SendToBankResultVm(Message:"بانک مذکور یافت نشد");
        }

        public string SendToPasianPaymentGateway(SelectPaymentDetailResult paymentDetail, string onlineId)
        {

            throw new NotImplementedException();
        }
    }
}
