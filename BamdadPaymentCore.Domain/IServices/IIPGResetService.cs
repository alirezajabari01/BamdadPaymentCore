using RestService.models;
using RestService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestService.models.verify;
using RestService.models.settle;
using RestService.models.reverse;
using RestService.models.bill;
using BamdadPaymentCore.Domain.StoreProceduresModels.Response;

namespace BamdadPaymentCore.Domain.IServices
{
    public interface IIPGResetService
    {
        public Task<TResult> Token<TRequest, TResult>(TRequest request, DataTable dt)
            where TRequest : ITokenCommand
            where TResult : class, ITokenVm, new();
        public Task<TResult> GetToken<TRequest, TResult>(TRequest request, SelectPaymentDetailResult paymentDetail)
            where TRequest : ITokenCommand
            where TResult : class, ITokenVm, new();

        public Task<VerifyVm> VerifyTrx(VerifyCommand verifyCommand, DataTable dt);
        public Task<VerifyVm> VerifyTrx(VerifyCommand verifyCommand, SelectPaymentDetailResult paymentDetail);

        public Task<SettleVm> SettleTrx(SettleCommand settleCommand, DataTable dt);
        public Task<SettleVm> SettleTrx(SettleCommand settleCommand, SelectPaymentDetailResult paymentDetail);

        public Task<ReverseVm> ReverseTrx(ReverseCommand reverseCommand);

        public Task<PaymentResultVm> TranResult(int merchantConfigId, long localInvoiceId, DataTable dt);
        public Task<PaymentResultVm> TranResult(int merchantConfigId, long localInvoiceId, SelectPaymentDetailResult paymentDetail);
    }
}
