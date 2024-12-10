using BamdadPaymentCore.Domain.Models.StoreProceduresModels;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public record UpdateOnlinePayRefundParameter(int Online_ID, int Online_ErrorCode)
        : StoreProcedureRequestModel;

}
