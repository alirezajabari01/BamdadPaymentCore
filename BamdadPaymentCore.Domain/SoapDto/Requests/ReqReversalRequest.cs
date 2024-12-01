namespace BamdadPaymentCore.Domain.SoapDto.Requests
{
    public record ReqReversalRequest(string Username, string Password, string OnlineId);
    public class ReqReversalRequestMappe
    {
        public static ReqReversalRequest ToReqReversalRequest(string username, string password, string onlineId)
            => new(username, password, onlineId);
    }
}



