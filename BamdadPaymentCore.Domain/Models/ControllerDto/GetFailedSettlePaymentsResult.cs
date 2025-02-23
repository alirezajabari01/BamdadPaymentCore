namespace BamdadPaymentCore.Domain.Models.ControllerDto
{
    public class GetFailedSettlePaymentsResult
    {
        public int Online_ID { get; set; }
        public string Bank_MerchantID { get; set; }
        public string Online_TransactionNo { get; set; }
        public string Online_OrderNo { get; set; }
        public string CardHolderInfo { get; set; }
        public string ReferenceNumber { get; set; }
        public string Bank_User { get; set; }
        public string Bank_Pass { get; set; }
        public string Site_ReturnUrl { get; set; }
    }
}
