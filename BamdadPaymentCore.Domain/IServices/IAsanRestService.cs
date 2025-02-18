
using Microsoft.AspNetCore.Http;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models;

using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.settlement;
using BamdadPaymentCore.Domain.Models.ControllerDto;

namespace BamdadPaymentCore.Domain.IServices
{
    public interface IAsanRestService : IBankService
    {
       // public void ProcessPayFailedVerifyPayments(GetFailedVerifyPaymentsResult faileds);

        public Task PayFailedVerifyPayments();

        public Task<TResult> GetToken<TRequest, TResult>(TRequest request, SelectPaymentDetailResult paymentDetail)
            where TRequest : ITokenCommand where TResult : class, ITokenVm, new();

        AsanTransactionResult GetTransationResultFromAsanPardakht(string onlineId, SelectPaymentDetailResult paymentDetail);

        SettleVm SettleAsan(AsanTransactionResult tranResult, SelectPaymentDetailResult paymentDetail, string onlineId);

        string ProcessAsanPardakhtPayment(string onlineId);

        string SendToAsanPardakhtPaymentGateway(SelectPaymentDetailResult paymentDetail, string onlineId);

        bool Cancel(string onlineId);
    }
}
