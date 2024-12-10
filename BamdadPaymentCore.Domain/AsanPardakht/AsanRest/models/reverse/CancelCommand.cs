namespace BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.reverse
{
    public class CancelCommand
    {
        public int merchantConfigurationId { get; set; }
        public ulong payGateTranId { get; set; }
    }

}