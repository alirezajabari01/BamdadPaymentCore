namespace BamdadPaymentCore.Domain.SoapDto.Requests
{
    public record SettleOnlineRequest(string Username, string Password, string OnlineId);
}
