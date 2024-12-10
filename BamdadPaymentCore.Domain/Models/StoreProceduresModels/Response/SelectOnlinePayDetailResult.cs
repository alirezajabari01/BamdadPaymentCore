using BamdadPaymentCore.Domain.Models.StoreProceduresModels;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response
{
    public class SelectOnlinePayDetailResult : StoreProcedureResponseModel
    {
        public bool Online_Status { get; set; }
        public int Online_ReqID { get; set; }
        public int Online_Kind { get; set; }
        public int Online_Price { get; set; }
        public int IsSettle { get; set; }
        public string Online_OrderNo { get; set; }
    }
}
