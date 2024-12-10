namespace BamdadPaymentCore.Domain.Models.SoapDto.Requests
{
    public record GetOnlineIdWithSettleRequest(string Username, string Password, string Price, string Desc, string ReqId, string Kind);

    public class GetOnlineIdWithSettleRequestMapper
    {
        public static GetOnlineIdWithSettleRequest ToGetOnlineIdWithSettleRequest(string username, string password, string price, string desc, string reqId, string kind) => new(username, password, price, desc, reqId, kind);
    }
}



