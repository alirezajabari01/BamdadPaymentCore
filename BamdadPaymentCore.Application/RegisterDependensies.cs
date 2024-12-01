using BamdadPaymentCore.Application.Contract.IServices;
using BamdadPaymentCore.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Application
{
    public static class RegisterDependensies
    {
        public static IServiceCollection RegisterApplication(this IServiceCollection services)
        => services.AddScoped<IPaymentService, PaymentService>();
    }
}
