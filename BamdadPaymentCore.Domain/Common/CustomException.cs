using BamdadPaymentCore.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Common
{
    public class PaymentDetailException() : AppException("Invalid payment details provided.", HttpStatusCode.BadRequest);

    public class UpdateOnlinePaymentException() : AppException("UpdateOnlinePay Failed ", HttpStatusCode.InternalServerError);

    public class GetTransationResultException() : AppException("Get TransationResult Failed ", HttpStatusCode.InternalServerError);

    public class VerifyTransationException() : AppException(" Verify Transation Failed ", HttpStatusCode.InternalServerError);

    public class ReverseTransationException() : AppException(" Reverse Transation Failed ", HttpStatusCode.InternalServerError);

    public class CancelTransationException() : AppException(" Cancel Transation Failed ", HttpStatusCode.InternalServerError);

    public class OnlineIdNotFoundException() : AppException(" OnlineId NotFound ", HttpStatusCode.NotFound);

    public class SiteAuthenticationException() : AppException(" Authentication Failed ", HttpStatusCode.Unauthorized);
}
