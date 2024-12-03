using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.IServices
{
    public interface IReturnBankWithAcceptService
    {
        string RedirectToUrl(string referenceNumber ,string onlineId, string refId, string saleReferenceId, string resCode, string cardInfo);
    }
}
