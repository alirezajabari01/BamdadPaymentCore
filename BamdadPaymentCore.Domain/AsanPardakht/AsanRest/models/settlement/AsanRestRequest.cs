namespace BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.settlement
{
    public class AsanRestRequest
    {
        public int merchantConfigurationId { get; set; }
        public ulong payGateTranId { get; set; }
        public string BankUser { get; set; }
        public string BankPassword { get; set; }
    }
}