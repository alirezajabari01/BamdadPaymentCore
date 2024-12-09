using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models
{
    public record AsanTransactionResult(string refId, string saleReferenceId, string cardHolderInfo, string referenceNumber, string resMessage, int resCode);
  
}
