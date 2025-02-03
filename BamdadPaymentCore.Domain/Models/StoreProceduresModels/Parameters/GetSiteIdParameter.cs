using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.Models.ServicesModels;
using Microsoft.Data.SqlClient;


namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public record GetSiteIdParameter(SqlParameter[] GetSiteIdSqlParameter);
    
    public static class GetSiteIdParameterMapper
    {
        public static GetSiteIdParameter ToGetSiteIdParameter(this GetSiteIdRequest request)
           => new
           (
               [
                   new("@UserName",request.UserName),
                   new("@Password",Helper.HashMd5(request.Password))
               ]
           );
    }
}
