namespace BamdadPaymentCore.Domain.Models.SoapDto.Requests
{
    public record SettleOnlineRequest(string Username, string Password, string OnlineId);
}
