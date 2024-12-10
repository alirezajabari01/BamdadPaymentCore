using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Models.SoapDto.Requests
{
    public record VerifyRequest(string Username, string Password, string OnlineId);
}
