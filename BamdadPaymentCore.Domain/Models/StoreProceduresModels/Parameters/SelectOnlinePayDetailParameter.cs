using BamdadPaymentCore.Domain.Models.StoreProceduresModels;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public record SelectOnlinePayDetailParameter(string Online_ID) : StoreProcedureRequestModel;

}
