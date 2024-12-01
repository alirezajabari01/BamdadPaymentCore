namespace BamdadPaymentCore.Domain.StoreProceduresModels.Parameters
{
    public record InsertOnlinePayParameter(int Bank_ID, int? Site_ID, int Online_Price,
        string Online_Desc, int Online_ReqID, int Online_Kind, int IsSettle, string Online_Type) : StoreProcedureRequestModel;
}
