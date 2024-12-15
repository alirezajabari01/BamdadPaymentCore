using BamdadPaymentCore.Domain.Exceptions;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;

namespace BamdadPaymentCore.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                var bamdadPaymentRepository = context.RequestServices.GetService<IBamdadPaymentRepository>();

                bamdadPaymentRepository?.insertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Message));

                context.Response.StatusCode = (int)ex.HttpStatusCode;
                await context.Response.WriteAsync(ex.Message);

            }
            catch (Exception ex)
            {
                var bamdadPaymentRepository = context.RequestServices.GetService<IBamdadPaymentRepository>();

                bamdadPaymentRepository?.insertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Message));

                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
