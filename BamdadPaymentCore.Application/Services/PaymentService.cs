using BamdadPaymentCore.Application.Contract.Dtos.Requests;
using BamdadPaymentCore.Application.Contract.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Application.Services
{
    public class PaymentService : IPaymentService
    {

        public string GetOnlineId(GetOnlineIdRequest request)
        {
            throw new NotImplementedException();
        }

        public string GetOnlineIdkind(GetOnlineIdkindRequest request)
        {
            throw new NotImplementedException();
        }

        public string GetOnlineIdWithSettle(GetOnlineIdWithSettleRequest request)
        {
            throw new NotImplementedException();
        }

        public DataTable GetOnlineStatus(GetOnlineStatusRequest request)
        {
            throw new NotImplementedException();
        }

        public bool ReqRefund(ReqRefundRequest request)
        {
            throw new NotImplementedException();
        }

        public bool ReqReversal(ReqReversalRequest request)
        {
            throw new NotImplementedException();
        }

        public DataTable ReqSettleOnline(ReqSettleOnlineRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
