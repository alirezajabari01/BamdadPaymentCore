namespace BamdadPaymentCore.Domain.SoapDto.Requests
{
    public record ReqRefundRequest(string Username, string Password, string OnlineId, string RefundAmount);
    public class ReqRefundRequestMapper
    {
        public static ReqRefundRequest ToReqRefundRequest(string username, string password, string onlineId, string refundAmount)
            => new(username, password, onlineId, refundAmount);
    }
}



