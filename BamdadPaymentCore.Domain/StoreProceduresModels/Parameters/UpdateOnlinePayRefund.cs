namespace BamdadPaymentCore.Domain.StoreProceduresModels.Parameters
{
    public record UpdateOnlinePayRefundParameter(int Online_ID, int Online_ErrorCode)
        : StoreProcedureRequestModel;

}
