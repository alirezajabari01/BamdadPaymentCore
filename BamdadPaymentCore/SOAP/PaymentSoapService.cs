using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.SoapDto.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.SOAP
{
    public class PaymentSoapService(IPaymentService paymentService) : IPaymentSoapService
    {
        public string GetOnlineId(string username, string pass, string price, string desc, string reqId)
           => paymentService.GetOnlineId(GetOnlineIdRequestMapper.ToGetOnlineIdRequest(username, pass, price, desc, reqId));
      
        public string GetOnlineIdkind(string username, string pass, string price, string desc, string reqId, string kind)
            => paymentService.GetOnlineIdkind(GetOnlineIdkindRequestMapper.ToGetOnlineIdkindRequest(username, pass, price, desc, reqId, kind));

        public string GetOnlineIdWithSettle(string username, string pass, string price, string desc, string reqId, string kind)
            => paymentService.GetOnlineIdWithSettle(GetOnlineIdWithSettleRequestMapper.ToGetOnlineIdWithSettleRequest(username, pass, price, desc, reqId, kind));

        public DataTable GetOnlineStatus(string username, string pass, string onlineId)
            => paymentService.GetOnlineStatus(GetOnlineStatusRequestMapper.ToGetOnlineStatusRequest(username, pass, onlineId));

        public bool ReqRefund(string username, string pass, string onlineId, string refundAmount)
            => paymentService.ReqRefund(ReqRefundRequestMapper.ToReqRefundRequest(username, pass, onlineId, refundAmount));

        public bool ReqReversal(string username, string pass, string onlineId)
            => paymentService.ReqReversal(ReqReversalRequestMappe.ToReqReversalRequest(username, pass, onlineId));

        public DataTable ReqSettleOnline(string username, string pass, string onlineId)
            => paymentService.ReqSettleOnline(ReqSettleOnlineRequestMapper.ToReqSettleOnlineRequest(username, pass, onlineId));
    }
}
