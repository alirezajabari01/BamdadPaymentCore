using BamdadPaymentCore.Domain.Models.ControllerDto;
using Microsoft.Data.SqlClient;


namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public record GetPaymentReportParameter(SqlParameter[] Parameters);


    public static class GetPaymentReportParameterMapper
    {
        public static GetPaymentReportParameter ToGetPaymentReportParameter(this GetPaymentReportRequest request, int siteId)
            => new
            (
                [
                    new("@StartDate",request.StartDate),
                    new("@EndDate",request.EndDate),
                    new("@Status",request.Status),
                    new("@SiteId",siteId),
                ]
            );
    }
}
