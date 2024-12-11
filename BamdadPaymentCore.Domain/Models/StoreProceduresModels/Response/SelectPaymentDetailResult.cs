using BamdadPaymentCore.Domain.Models.StoreProceduresModels;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response
{
    public class SelectPaymentDetailResult : StoreProcedureResponseModel
    {
        public int Online_Price { get; set; }
        public string Bank_MerchantID { get; set; }
        public string Bank_User { get; set; }
        public string Bank_Pass { get; set; }
        public string BankCode { get; set; }
        public int Online_Kind { get; set; }
        public int IsSettle { get; set; }
        public bool Online_Status { get; set; }
        public bool AutoSettle { get; set; }
        public string? User_mobile { get; set; }
    }
}
