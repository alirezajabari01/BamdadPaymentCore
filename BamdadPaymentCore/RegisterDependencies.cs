using BamdadPaymentCore.SOAP;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SoapCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore
{
    public static class RegisterDependencies
    {
        public static void RegisterSoap(this IServiceCollection services)
        {
            services.AddSoapCore();
            services.AddScoped<IPaymentSoapService, PaymentSoapService>();
        }
        public static void RegisterSoap(this WebApplication app)
        {
            app.UseSoapEndpoint<IPaymentSoapService>("/PaymentSoapService.asmx", new SoapEncoderOptions());
        }
    }
}
