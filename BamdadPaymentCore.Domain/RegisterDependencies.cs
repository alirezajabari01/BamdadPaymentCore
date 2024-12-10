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
using System;
using System.Collections.Generic;
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
        }

        public static void ConfigureOptionPattern(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("Sql");

            builder.Services.AddDbContext<NimkatOnlineContext>(options => options.UseSqlServer(connectionString));

            builder.Services.Configure<PaymentGatewaySetting>(builder.Configuration.GetSection("PaymentSettings"));
        }
    }
}
