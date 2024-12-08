//using BamdadPaymentCore.SOAP;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.DependencyInjection;
//using SoapCore;
//using System.IO;
//using System.Text;
//using System.Threading.Tasks;
//namespace BamdadPaymentCore
//{


//    public class CustomWsdlMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public CustomWsdlMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            if (context.Request.Path.StartsWithSegments("/PaymentSoapService"))
//            {
//                // Intercept the WSDL request and modify it
//                var wsdl = await GenerateCustomWsdl(context);

//                // Modify the targetNamespace
//                wsdl = wsdl.Replace("http://tempuri.org/", "http://www.pay.local/");

//                // Write the modified WSDL back to the response
//                context.Response.ContentType = "application/xml";
//                await context.Response.WriteAsync(wsdl);
//            }
//            else
//            {
//                await _next(context);
//            }
//        }

//        private async Task<string> GenerateCustomWsdl(HttpContext context)
//        {
//            // You can use SoapCore to generate the WSDL dynamically
//            var soapEndpoint = context.Request.Path.ToString();
//            var wsdlService = context.RequestServices.GetRequiredService<IPaymentSoapService>();

//            // Generate the WSDL with SoapCore dynamically
//            var wsdl = await wsdlService.GetWSDL(soapEndpoint);

//            return wsdl;
//        }
//    }

//}
