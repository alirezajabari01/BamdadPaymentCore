using BamdadPaymentCore.Domain.BackgoundJobs.BaseQuartz;
using BamdadPaymentCore.Domain.IServices;
using Quartz;

namespace BamdadPaymentCore.Domain.BackgoundJobs
{
    public class HandleFailedInBankVerifyJob : AsanRestServiceBaseQuartz
    {
        public HandleFailedInBankVerifyJob(IAsanRestService asanRestService) : base(asanRestService)
        {
        }

        public override async Task Execute(IJobExecutionContext context)
        {
            await _asanRestService.PayFailedVerifyPayments();
        }
    }
}
