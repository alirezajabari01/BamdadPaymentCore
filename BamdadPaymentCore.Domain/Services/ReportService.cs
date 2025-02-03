using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.Models.ControllerDto;
using BamdadPaymentCore.Domain.Models.ServicesModels;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Services
{
    public class ReportService(IBamdadPaymentRepository repository) : IReportService
    {
        public ApiResult<List<GetPaymentReportResponse>> GetPaymentReport(GetPaymentReportRequest request)
        {
            var getSiteId = request.ToGetSiteIdRequest();
            var siteId = GetSiteId(getSiteId);

            var spParameter = request.ToGetPaymentReportParameter(siteId);
            var response = repository.PaymentReport(spParameter);

            return SucceedApiResult<List<GetPaymentReportResponse>>.Create(response);
        }

        public int GetSiteId(GetSiteIdRequest request)
        => repository.GetSiteId(request.ToGetSiteIdParameter()) ?? throw new GetSiteIdException();
    }
}
