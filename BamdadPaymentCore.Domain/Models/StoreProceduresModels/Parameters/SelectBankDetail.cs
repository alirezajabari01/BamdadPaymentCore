using BamdadPaymentCore.Domain.Models.StoreProceduresModels;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public record SelectBankDetailParameter(string Online_ID) : StoreProcedureRequestModel;
}
