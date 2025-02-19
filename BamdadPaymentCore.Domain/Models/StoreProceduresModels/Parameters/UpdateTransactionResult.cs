namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public class UpdateTransactionResultParameter
    {
        public UpdateTransactionResultParameter(
            string referenceNumber,
            string online_ID,
            string errorCode,
            string online_TransactionNo = "Failed",
            string online_OrderNo = "Failed",
            string cardHolderInfo = ""
        )
        {
            ReferenceNumber = referenceNumber;
            Online_ID = Convert.ToInt32(online_ID);
            ErrorCode = errorCode;
            Online_TransactionNo = online_TransactionNo ?? "failed";
            Online_OrderNo = online_OrderNo ?? "failed";
            CardHolderInfo = cardHolderInfo ?? "";
        }

        public string ReferenceNumber { get; set; }
        public int Online_ID { get; set; }
        public string Online_TransactionNo { get; set; }
        public string Online_OrderNo { get; set; }
        public string CardHolderInfo { get; set; }
        public string ErrorCode { get; set; }
    }

    
}
