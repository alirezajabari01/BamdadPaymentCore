
using BamdadPaymentCore.Domain.Database;
using BamdadPaymentCore.Domain.IRepositories;

namespace BamdadPaymentCore.BackgroundJobs
{
    public class AutoSettleBackgroundJob : BackgroundService
    {
        private readonly NimkatOnlineContext context;

        public AutoSettleBackgroundJob(IServiceScopeFactory serviceScopeFactory)
        {
            context = serviceScopeFactory.CreateScope().ServiceProvider.GetService<NimkatOnlineContext>()
                       ?? throw new InvalidOperationException();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ////TODO check if cancel time duration 3h , auto settle by bank 30min
            //while (true)
            //{
            //    context.OnlinePay.Where
            //        (
            //        pay => pay.OnlineStatus == true &&
            //        pay.AutoSettle == false &&
            //        pay.RefundStatus != 1
            //        )
            //        .ToList()
            //        .ForEach(pay => { pay.AutoSettle = true; });

            //    await Task.Delay(TimeSpan.FromMinutes(31));
            //}

        }
    }
}
