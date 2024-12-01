namespace BamdadPaymentCore.Domain.StoreProceduresModels.Response
{
    public class SelectOnlinePayResult : StoreProcedureResponseModel
    {
        public int Online_ID { get; set; }
        public string Bank_Name { get; set; }
        public int Price { get; set; }
        public string OnlineDate { get; set; }
        public bool Status { get; set; }
        public int IsSettle { get; set; }


    }
}
