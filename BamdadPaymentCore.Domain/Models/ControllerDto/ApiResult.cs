using BamdadPaymentCore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BamdadPaymentCore.Domain.Models.ControllerDto
{

    public class ApiResult<TResult> where TResult : class
    {
        public TResult Data { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public string Message { get; set; }

        public ApiResult(TResult data, HttpStatusCode httpStatusCode)
        {
            Data = data;
            HttpStatusCode = httpStatusCode;
        }
        public ApiResult(TResult data, HttpStatusCode httpStatusCode, string message)
        {
            Data = data;
            HttpStatusCode = httpStatusCode;
            Message = message;
        }
        public static ApiResult<TResult> Success(TResult data, HttpStatusCode httpStatusCode = HttpStatusCode.OK) => new ApiResult<TResult>(data, httpStatusCode);
    }

    public class SucceedApiResult<TResult> : ApiResult<TResult> where TResult : class
    {
        public SucceedApiResult(TResult data, HttpStatusCode httpStatusCode = HttpStatusCode.OK) : base(data, httpStatusCode)
        {
        }
        public static SucceedApiResult<TResult> Create(TResult data, HttpStatusCode httpStatusCode = HttpStatusCode.OK) => new SucceedApiResult<TResult>(data, httpStatusCode);

    }

    //public class FailedApiResult<TResult> : ApiResult<TResult>
    //{
    //    public FailedApiResult(TResult data, HttpStatusCode httpStatusCode, string message = "خطایی پیش آمده") : base(data, httpStatusCode, message)
    //    {
    //    }
    //    public static FailedApiResult<TResult> Create<TResult>(TResult data, HttpStatusCode httpStatusCode, string message = "خطایی پیش آمده") 
    //        => new FailedApiResult<TResult>(data, httpStatusCode, message);

    //}
}
