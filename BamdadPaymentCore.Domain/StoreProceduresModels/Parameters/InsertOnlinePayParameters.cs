namespace BamdadPaymentCore.Domain.StoreProceduresModels.Parameters
{
    public record InsertIntoOnlinePayParameter(int Bank_ID, int? Site_ID, int Online_Price,
        string Online_Desc, int Online_ReqID, int Online_Kind, bool AutoSettle , string Online_Type) : StoreProcedureRequestModel;
}
