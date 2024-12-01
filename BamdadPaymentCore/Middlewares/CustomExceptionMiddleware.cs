using BamdadPaymentCore.Domain.Exceptions;

namespace BamdadPaymentCore.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        //TODO Site Error Should Be Added Here Too 
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
                var statusCode = ex.HttpStatusCode;
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
