namespace BamdadPaymentCore.Domain.StoreProceduresModels.Parameters
{
    public record UpdateOnlinePayRefundParameter(string Online_ID, string Online_ErrorCode)
        : StoreProcedureRequestModel;

}
