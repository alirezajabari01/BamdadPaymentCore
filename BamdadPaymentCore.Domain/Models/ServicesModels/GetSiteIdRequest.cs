using BamdadPaymentCore.Domain.Models.ControllerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Models.ServicesModels
{
    public record GetSiteIdRequest(string UserName, string Password);

    public static class GetSiteIdRequestMapper
    {
        public static GetSiteIdRequest ToGetSiteIdRequest(this GetPaymentReportRequest request)
            => new GetSiteIdRequest(request.UserName, request.Password);
    }
}
