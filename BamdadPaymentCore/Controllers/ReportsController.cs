using BamdadPaymentCore.Controllers.Base;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.Models.ControllerDto;
using Microsoft.AspNetCore.Mvc;

namespace BamdadPaymentCore.Controllers
{

    public class ReportsController(IReportService reportService) : ApiBaseController
    {
        [HttpGet]
        public ApiResult<List<GetPaymentReportResponse>> GetReport([FromBody] GetPaymentReportRequest request)
         => reportService.GetPaymentReport(request);
    }
}
