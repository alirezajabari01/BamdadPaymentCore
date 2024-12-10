using BamdadPaymentCore.Domain.Models.StoreProceduresModels;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public record UpdateOnlinePaySettleFailedParameter(string Online_ID) : StoreProcedureRequestModel;

}
