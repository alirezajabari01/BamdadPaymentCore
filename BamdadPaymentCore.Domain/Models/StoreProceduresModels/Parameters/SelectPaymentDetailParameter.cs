using BamdadPaymentCore.Domain.Models.StoreProceduresModels;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public record SelectPaymentDetailParameter(string Online_ID) : StoreProcedureRequestModel;
}
