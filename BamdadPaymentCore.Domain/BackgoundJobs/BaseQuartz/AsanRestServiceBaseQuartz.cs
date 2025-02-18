using BamdadPaymentCore.Domain.Database;
using BamdadPaymentCore.Domain.IServices;
using LMSApi.BackgoundJobs.BaseQuartz;


namespace BamdadPaymentCore.Domain.BackgoundJobs.BaseQuartz
{
    public abstract class AsanRestServiceBaseQuartz(IAsanRestService asanRestService) : QuartzBaseClass
    {
        protected readonly IAsanRestService _asanRestService = asanRestService;
    }
}
