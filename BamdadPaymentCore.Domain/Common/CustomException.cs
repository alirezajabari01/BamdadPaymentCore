using BamdadPaymentCore.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Common
{
    public class PaymentDetailException : AppException
    {
        public PaymentDetailException() : base("Invalid payment details provided.", HttpStatusCode.BadRequest) { }
    }

    public class UpdateOnlinePaymentException : AppException
    {
        public UpdateOnlinePaymentException() : base("UpdateOnlinePay Failed ", HttpStatusCode.InternalServerError) { }
    }

    public class GetTransationResultException : AppException
    {
        public GetTransationResultException() : base("Get TransationResult Failed ", HttpStatusCode.InternalServerError) { }
    }

    public class VerifyTransationException : AppException
    {
        public VerifyTransationException() : base(" Verify Transation Failed ", HttpStatusCode.InternalServerError) { }
    }

    public class ReverseTransationException : AppException
    {
        public ReverseTransationException() : base(" Reverse Transation Failed ", HttpStatusCode.InternalServerError) { }
    }

    public class CancelTransationException : AppException
    {
        public CancelTransationException() : base(" Cancel Transation Failed ", HttpStatusCode.InternalServerError) { }
    }

    public class OnlineIdNotFoundException : AppException
    {
        public OnlineIdNotFoundException() : base(" OnlineId NotFound ", HttpStatusCode.NotFound) { }
    }
}
