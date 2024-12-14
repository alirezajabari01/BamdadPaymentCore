using BamdadPaymentCore.Domain.Models.StoreProceduresModels;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response
{
    public class SelectOnlinePayDetailResult 
    {
        public int Online_ID { get; set; }
        public int Bank_ID { get; set; }
        public int Site_ID { get; set; }
        public string Online_TransactionNo { get; set; }
        public string Online_OrderNo { get; set; }
        public int Online_Price { get; set; }
        public DateTime Online_CreateDate { get; set; }
        public bool Online_Status { get; set; }
        public string Online_ErrorCode { get; set; }
        public string Online_Desc { get; set; }
        public int Online_ReqID { get; set; }
        public int Online_Kind { get; set; }
        public int IsSettle { get; set; }
        public string Online_Type { get; set; }
        public int refundStatus { get; set; }
        public string CardHolderInfo { get; set; }
        public string ReferenceNumber { get; set; }
        public bool AutoSettle { get; set; }
        public string MobileNomber { get; set; }
    }
}
