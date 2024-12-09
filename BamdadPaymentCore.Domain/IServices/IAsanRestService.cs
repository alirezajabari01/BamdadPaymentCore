using BamdadPaymentCore.Domain.StoreProceduresModels.Response;
using RestService.models;
using RestService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models;
using RestService.models.settle;
using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.Services;
using BamdadPaymentCore.Domain.StoreProceduresModels.Parameters;

namespace BamdadPaymentCore.Domain.IServices
{
    public interface IAsanRestService : IBankService
    {
        public Task<TResult> GetToken<TRequest, TResult>(TRequest request, SelectPaymentDetailResult paymentDetail)
            where TRequest : ITokenCommand where TResult : class, ITokenVm, new();

        AsanTransactionResult GetTransationResultFromAsanPardakht(string onlineId, SelectPaymentDetailResult paymentDetail);

        public string Return(HttpRequest Request);

        SettleVm SettleAsan(AsanTransactionResult tranResult, SelectPaymentDetailResult paymentDetail, string onlineId);

        string ProcessAsanPardakhtPayment(string onlineId);

        string SendToAsanPardakhtPaymentGateway(SelectPaymentDetailResult paymentDetail, string onlineId);

        bool Cancel(string onlineId);
    }
}
