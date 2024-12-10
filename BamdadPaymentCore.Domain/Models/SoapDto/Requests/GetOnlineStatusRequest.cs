namespace BamdadPaymentCore.Domain.Models.SoapDto.Requests
{
    public record GetOnlineStatusParameter(string Username, string Password, string OnlineId);
    public class GetOnlineStatusRequestMapper
    {
        public static GetOnlineStatusParameter ToGetOnlineStatusRequest(string username, string password, string onlineId)
            => new(username, password, onlineId);
    }
}



