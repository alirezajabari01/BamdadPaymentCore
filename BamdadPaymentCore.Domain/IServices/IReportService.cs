using BamdadPaymentCore.Domain.Models.ControllerDto;
using BamdadPaymentCore.Domain.Models.ServicesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.IServices
{
    public interface IReportService
    {
        public ApiResult<List<GetPaymentReportResponse>> GetPaymentReport(GetPaymentReportRequest request);

        public int GetSiteId(GetSiteIdRequest request);
    }
}
