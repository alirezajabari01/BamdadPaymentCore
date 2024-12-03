using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.StoreProceduresModels.Parameters
{
    public record UpdateOnlinePayWithSettleParameter(int Online_ID, string Online_TransactionNo = "Failed", string Online_OrderNo = "Failed", int Online_ErrorCode = 0, string CardHolderInfo = "");
}
