namespace BamdadPaymentCore.Domain.StoreProceduresModels.Parameters
{
    public record UpdateOnlinePayResWithSettleFailedParameter(string Online_ID, int Online_ErrorCode) : StoreProcedureRequestModel;
}
