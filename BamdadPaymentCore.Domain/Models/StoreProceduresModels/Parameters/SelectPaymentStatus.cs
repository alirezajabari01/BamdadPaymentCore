using BamdadPaymentCore.Domain.Models.StoreProceduresModels;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public record SelectPaymentStatus(string Online_ID) : StoreProcedureRequestModel;

}
