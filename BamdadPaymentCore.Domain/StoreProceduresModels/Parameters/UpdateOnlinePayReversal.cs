namespace BamdadPaymentCore.Domain.StoreProceduresModels.Parameters
{
    public record UpdateOnlinePayReversalParameter(string Online_ID, string Online_ErrorCode)
        : StoreProcedureRequestModel;

}
