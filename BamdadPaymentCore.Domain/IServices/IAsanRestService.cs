
using Microsoft.AspNetCore.Http;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models;

using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.settlement;

namespace BamdadPaymentCore.Domain.IServices
{
    public interface IAsanRestService : IBankService
    {
        public Task<TResult> GetToken<TRequest, TResult>(TRequest request, SelectPaymentDetailResult paymentDetail)
            where TRequest : ITokenCommand where TResult : class, ITokenVm, new();

        AsanTransactionResult GetTransationResultFromAsanPardakht(string onlineId, SelectPaymentDetailResult paymentDetail);

        public string ProcessCallBackFromBank(HttpRequest Request);

        SettleVm SettleAsan(AsanTransactionResult tranResult, SelectPaymentDetailResult paymentDetail, string onlineId);

        string ProcessAsanPardakhtPayment(string onlineId);

        string SendToAsanPardakhtPaymentGateway(SelectPaymentDetailResult paymentDetail, string onlineId);

        bool Cancel(string onlineId);
    }
}
