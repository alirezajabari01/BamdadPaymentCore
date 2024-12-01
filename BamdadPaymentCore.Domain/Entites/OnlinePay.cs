namespace BamdadPaymentCore.Domain.Entites
{
    public class OnlinePay
    {
        public int OnlineId { get; set; }
        public int BankId { get; set; }
        public int SiteId { get; set; }
        public string? OnlineTransactionNo { get; set; }
        public string? OnlineOrderNo { get; set; }
        public int OnlinePrice { get; set; }
        public DateTime? OnlineCreateDate { get; set; }
        public bool? OnlineStatus { get; set; }
        public string? OnlineErrorCode { get; set; }
        public string? OnlineDesc { get; set; }
        public int OnlineReqId { get; set; }
        public int OnlineKind { get; set; }
        public int IsSettle { get; set; }
        public string? OnlineType { get; set; }
        public int RefundStatus { get; set; }
        public string? CardHolderInfo { get; set; }
    }
}
