using Autofac.Core;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest;
using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.Database;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.Repositories;
using BamdadPaymentCore.Domain.Services;
using bpm.shaparak.ir;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain
{
    public static class RegisterDependencies
    {
        public static void RegisterDomainDependencies(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<IBamdadPaymentRepository, BamdadPaymentRepository>();

            services.AddScoped<IPaymentGateway, PaymentGatewayClient>();

            services.AddScoped<IMellatService, MellatService>();

            services.AddScoped<ISendToBankService, SendToBankService>();

            services.AddScoped<IAsanRestService, AsanResetService>();

            services.AddScoped<IReturnBankWithAcceptService, ReturnBankWithAcceptService>();

            services.AddScoped<IReportService, ReportService>();
        }

        public static void RegisterQuartz(this WebApplicationBuilder builder)
        {
            try
            {
                var confs = builder.Configuration.GetSection("Quartz:Jobs").Get<List<JobConfiguration>>() ?? [];

                var jobs = typeof(RegisterDependencies).Assembly.GetTypes().Where(t => t.IsClass && t.IsAssignableTo(typeof(IJob)) && !t.IsAbstract);

                builder.Services.AddQuartz(opt =>
                {
                    foreach (var conf in confs)
                    {
                        if (conf.IsEnabled.Equals("false", StringComparison.CurrentCultureIgnoreCase)) continue;

                        var type = jobs.FirstOrDefault(d => d.Name == conf.JobKey);

                        if (type is null) throw new FileNotFoundException($"Job class '{conf.JobKey}' not found.");

                        var jobKey = new JobKey(conf.JobKey);

                        opt.AddJob(type, jobKey, opts => opts.WithIdentity(jobKey).StoreDurably());

                        var jobKey1 = new JobKey(conf.JobKey);

                        opt.AddTrigger(opts => opts.ForJob(jobKey1).WithIdentity(conf.WithIdentity).WithCronSchedule(conf.TimeRegex));
                    }
                });

                builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during Quartz setup: {ex.Message}");
            }
        }

        public static void ConfigureOptionPattern(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("Sql");

            builder.Services.AddDbContext<NimkatOnlineContext>(options => options.UseSqlServer(connectionString));

            builder.Services.Configure<PaymentGatewaySetting>(builder.Configuration.GetSection("PaymentSettings"));
        }
    }
    public class JobConfiguration
    {
        public string TimeRegex { get; set; }
        public string JobKey { get; set; }
        public string WithIdentity { get; set; }
        public string IsEnabled { get; set; }
    }
}
