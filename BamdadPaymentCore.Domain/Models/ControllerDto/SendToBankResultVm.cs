using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Models.ControllerDto
{
    public record SendToBankResultVm(string Url = "", string RefId = "");
}
