using BamdadPaymentCore.Application.Contract.Dtos;
using BamdadPaymentCore.Application.Contract.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Application.Contract.IServices
{
    public interface IPaymentService
    {
        string GetOnlineId(GetOnlineIdRequest request);
        string GetOnlineIdkind(GetOnlineIdkindRequest request);
        string GetOnlineIdWithSettle(GetOnlineIdWithSettleRequest request);
        DataTable GetOnlineStatus(GetOnlineStatusRequest request);
        bool ReqRefund(ReqRefundRequest request);
        bool ReqReversal(ReqReversalRequest request);
        DataTable ReqSettleOnline(ReqSettleOnlineRequest request);
    }
}
