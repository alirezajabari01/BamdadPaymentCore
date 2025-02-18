using Quartz;

namespace LMSApi.BackgoundJobs.BaseQuartz
{
    public abstract class QuartzBaseClass : IJob
    {
        public abstract Task Execute(IJobExecutionContext context);
    }
}
