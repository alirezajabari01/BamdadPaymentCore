namespace BamdadPaymentCore.Domain.SoapDto.Requests
{
    public record ReqSettleOnlineRequest(string Username, string Password, string OnlineId);
    public class ReqSettleOnlineRequestMapper
    {
        public static ReqSettleOnlineRequest ToReqSettleOnlineRequest(string username, string password, string onlineId)
           => new(username, password, onlineId);
    }
}



