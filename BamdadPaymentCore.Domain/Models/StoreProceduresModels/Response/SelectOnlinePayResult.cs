using BamdadPaymentCore.Domain.Models.StoreProceduresModels;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response
{
    public class SelectOnlinePayResult 
    {
        public int Online_ID { get; set; }
        public string Bank_Name { get; set; }
        public int Price { get; set; }
        public string OnlineDate { get; set; }
        public bool Status { get; set; }
        public int? IsSettle { get; set; }
        public string? OrderNo { get; set; }
    }
}
