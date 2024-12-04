namespace RestService.models.settle
{
    public class TransactionResultRequest
    {
        public TransactionResultRequest(string MerchantConfigId, string LocalInvoiceId, string BankUser, string BankPassword)
        {
            merchantConfigId = Convert.ToInt32(MerchantConfigId);
            localInvoiceId = long.Parse(LocalInvoiceId);
            bankUser = BankUser;
            bankPassword = BankPassword;
        }
        public int merchantConfigId { get; set; }
        public long localInvoiceId { get; set; }
        public string bankUser { get; set; }
        public string bankPassword { get; set; }
    }
}