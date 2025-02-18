namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public record UpdateIsSettleParameter
    {
        public UpdateIsSettleParameter(string onlineId,int isSettle)
        {
            OnlineId = Convert.ToInt32(onlineId);
            IsSettle = isSettle;
        }
        public int OnlineId { get; set; }
        public int IsSettle { get; set; }
    }

}
