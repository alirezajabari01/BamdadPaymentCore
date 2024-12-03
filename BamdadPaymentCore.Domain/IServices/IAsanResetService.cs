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
    public interface IAsanResetService
    {
        public Task<TResult> GetToken<TRequest, TResult>(TRequest request, SelectPaymentDetailResult paymentDetail)
            where TRequest : ITokenCommand
            where TResult : class, ITokenVm, new();

        public Task<VerifyVm> VerifyTransaction(VerifyCommand verifyCommand, SelectPaymentDetailResult paymentDetail);

        public Task<SettleVm> SettleTransaction(SettleCommand settleCommand, SelectPaymentDetailResult paymentDetail);

        public Task<ReverseVm> ReverseTransaction(ReverseCommand reverseCommand, string usr, string pwd);

        public Task<PaymentResultVm> TransactionResult(int merchantConfigId, long localInvoiceId, SelectPaymentDetailResult paymentDetail);

        public Task<CancelResultVm> CancelTransaction(CancelCommand command, string usr, string pwd);
    }
}
