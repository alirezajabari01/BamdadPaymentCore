namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public class UpdateTransactionResultParameter
    {
        public UpdateTransactionResultParameter(
            string referenceNumber,
            string online_ID,
            string online_TransactionNo = "Failed",
            string online_OrderNo = "Failed",
            string cardHolderInfo = ""
        )
        {
            ReferenceNumber = referenceNumber;
            Online_ID = Convert.ToInt32(online_ID);
            Online_TransactionNo = online_TransactionNo;
            Online_OrderNo = online_OrderNo;
            CardHolderInfo = cardHolderInfo;
        }

        public string ReferenceNumber { get; set; }
        public int Online_ID { get; set; }
        public string Online_TransactionNo { get; set; }
        public string Online_OrderNo { get; set; }
        public string CardHolderInfo { get; set; }
    }

    
}
