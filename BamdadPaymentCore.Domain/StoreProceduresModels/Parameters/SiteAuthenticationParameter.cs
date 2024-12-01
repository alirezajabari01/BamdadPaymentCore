namespace BamdadPaymentCore.Domain.StoreProceduresModels.Parameters
{
    public record SiteAuthenticationParameter(string Site_UserName, string Site_Pass, string Site_IP) : StoreProcedureRequestModel;
}
