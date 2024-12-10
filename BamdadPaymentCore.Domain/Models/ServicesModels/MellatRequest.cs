using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Models.ServicesModels
{
    public record MellatRequest(long terminalId, string userName, string userPassword, long orderId, long saleOrderId, long saleReferenceId);
}
