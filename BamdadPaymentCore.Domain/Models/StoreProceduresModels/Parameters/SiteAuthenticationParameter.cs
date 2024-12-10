using BamdadPaymentCore.Domain.Models.StoreProceduresModels;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public record SiteAuthenticationParameter(string Site_UserName, string Site_Pass, string Site_IP) : StoreProcedureRequestModel;
}
