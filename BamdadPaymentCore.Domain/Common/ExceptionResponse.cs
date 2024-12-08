using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Common
{
    public class ExceptionResponse
    {
        public const string PaymentDetailNotValid = "";
        public const string PaymentDetailNotFound = "";
        public const string UpdatePaymentFailed = "";



        public const int PaymentDetailNotValidStatusCode = 400;
        public const int PaymentDetailNotFoundStatusCode = 404;
        public const int UpdatePaymentFailedStatusCode = 404;
       
    }
}
