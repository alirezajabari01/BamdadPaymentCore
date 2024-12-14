using BamdadPaymentCore.Domain.Models.StoreProceduresModels;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public record SelectBankIdParameter(int? Site_ID) : StoreProcedureRequestModel;

}
