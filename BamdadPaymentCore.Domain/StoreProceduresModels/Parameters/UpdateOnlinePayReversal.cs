namespace BamdadPaymentCore.Domain.StoreProceduresModels.Parameters
{
    public record UpdateOnlinePayReversalParameter(int Online_ID, int Online_ErrorCode)
        : StoreProcedureRequestModel;

}
