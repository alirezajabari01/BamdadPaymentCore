using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;


namespace BamdadPaymentCore.Domain.Services
{
    internal class ReturnBankWithAcceptService(IPaymentService paymentService,IBamdadPaymentRepository repository) : IReturnBankWithAcceptService
    {
        public string RedirectToUrl(string referenceNumber ,string onlineId, string refId, string saleReferenceId, string resCode, string cardInfo)
        {
            cardInfo = cardInfo + "?OnlineID=" + onlineId;
            if (string.IsNullOrEmpty(onlineId)) return string.Empty;

            if (resCode != "0") return paymentService.UpdateOnlinePayFailed(referenceNumber, onlineId, refId, saleReferenceId, resCode, cardInfo);

            return repository.UpdateOnlinePayWithSettle(new UpdateOnlinePayWithSettleParameter(Convert.ToInt32(onlineId), refId, saleReferenceId, 0, cardInfo)).Site_ReturnUrl;
        }
    }
}
