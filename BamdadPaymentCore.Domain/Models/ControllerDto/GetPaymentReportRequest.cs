using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Models.ControllerDto
{
    public record GetPaymentReportRequest
    (
        string UserName,
        string Password,
        DateTime StartDate,
        DateTime EndDate,
        int Status
    );
}
