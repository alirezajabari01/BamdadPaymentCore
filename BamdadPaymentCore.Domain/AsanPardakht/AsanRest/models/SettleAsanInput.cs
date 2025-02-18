namespace BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models
{
    public record SettleAsanInput(string bankUser,string BankPass,string SaleReferenceId, int OnlineId);
}
