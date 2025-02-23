using BamdadPaymentCore.Domain.BackgoundJobs.BaseQuartz;
using BamdadPaymentCore.Domain.IServices;
using Quartz;

namespace BamdadPaymentCore.Domain.BackgoundJobs
{
    public class HandleFailedInBankSettleJob : AsanRestServiceBaseQuartz
    {
        public HandleFailedInBankSettleJob(IAsanRestService asanRestService) : base(asanRestService)
        {
        }

        public override async Task Execute(IJobExecutionContext context)
        {
          
        }
    }
}
