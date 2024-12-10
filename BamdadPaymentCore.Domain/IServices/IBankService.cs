using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.settlement;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.verify;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.bill;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.reverse;

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
