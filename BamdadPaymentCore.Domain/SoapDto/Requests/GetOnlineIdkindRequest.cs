namespace BamdadPaymentCore.Domain.SoapDto.Requests
{
    public record GetOnlineIdkindRequest(string Username, string Password, string Price, string Desc, string ReqId, string Kind);

    public class GetOnlineIdkindRequestMapper
    {
        public static GetOnlineIdkindRequest ToGetOnlineIdkindRequest(string username, string password, string price, string desc, string reqId, string kind)
           => new(username, password, price, desc, reqId, kind);
    }
}



