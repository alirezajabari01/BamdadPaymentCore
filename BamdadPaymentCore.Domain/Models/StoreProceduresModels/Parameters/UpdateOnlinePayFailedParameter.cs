﻿namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public class UpdateOnlinePayFailedParameter
    {
        public UpdateOnlinePayFailedParameter(
            string referenceNumber,
            string online_ID,
            string online_TransactionNo = "Failed",
            string online_OrderNo = "Failed",
            int online_ErrorCode = 0,
            string cardHolderInfo = ""
        )
        {
            ReferenceNumber = referenceNumber;
            Online_ID = Convert.ToInt32(online_ID);
            Online_TransactionNo = online_TransactionNo;
            Online_OrderNo = online_OrderNo;
            Online_ErrorCode = online_ErrorCode;
            CardHolderInfo = cardHolderInfo;
        }

        public string ReferenceNumber { get; set; }
        public int Online_ID { get; set; }
        public string Online_TransactionNo { get; set; }
        public string Online_OrderNo { get; set; }
        public int Online_ErrorCode { get; set; }
        public string CardHolderInfo { get; set; }
    }
    
}
