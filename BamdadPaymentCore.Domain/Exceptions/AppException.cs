using System.Net;

namespace BamdadPaymentCore.Domain.Exceptions;

public class AppException : Exception
{
    public HttpStatusCode HttpStatusCode { get; set; }

    public object AdditionalData { get; set; }

    public AppException() : this(HttpStatusCode.InternalServerError)
    {
    }

    public AppException(HttpStatusCode statusCode) : this(statusCode, null)
    {
    }

    public AppException(string message)
        : this(HttpStatusCode.InternalServerError, message)
    {
    }

    public AppException(HttpStatusCode statusCode, string message)
        : this(message, statusCode)
    {
    }

    public AppException(string message, object additionalData)
        : this(message, HttpStatusCode.InternalServerError, additionalData)
    {
    }

    public AppException(HttpStatusCode statusCode, object additionalData)
        : this(null, statusCode, additionalData)
    {
    }

    public AppException(string message, HttpStatusCode httpStatusCode)
        : this(message, httpStatusCode, null)
    {
    }

    public AppException(string message, HttpStatusCode httpStatusCode, object additionalData)
        : this(message, httpStatusCode, null, additionalData)
    {
    }

    public AppException(string message, Exception exception)
        : this(message, HttpStatusCode.InternalServerError, exception)
    {
    }

    public AppException(string message, Exception exception, object additionalData)
        : this(message, HttpStatusCode.InternalServerError, exception, additionalData)
    {
    }

    public AppException(string message, HttpStatusCode httpStatusCode, Exception exception)
        : this(message, httpStatusCode, exception, null)
    {
    }

    public AppException(string message, HttpStatusCode httpStatusCode, Exception exception, object additionalData)
        : base(message, exception)
    {
        HttpStatusCode = httpStatusCode;
        AdditionalData = additionalData;
    }
}
