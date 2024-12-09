using RestService.models;
using RestService;
using RestService.models.verify;
using RestService.models.settle;
using RestService.models.reverse;
using RestService.models.bill;
using BamdadPaymentCore.Domain.StoreProceduresModels.Response;

namespace BamdadPaymentCore.Domain.IServices
{
    public interface IBankService
    {
        public Task<VerifyVm> VerifyTransaction(AsanRestRequest requst);

        public Task<SettleVm> SettleTransaction(AsanRestRequest requst);

        public Task<ReverseVm> ReverseTransaction(AsanRestRequest requst);

        public Task<PaymentResultVm> TransactionResult(TransactionResultRequest request);

        public Task<CancelResultVm> CancelTransaction(AsanRestRequest requst);
    }
}
