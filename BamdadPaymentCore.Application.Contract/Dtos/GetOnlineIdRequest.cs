using BamdadPaymentCore.Application.Contract.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Application.Contract.Dtos.Requests
{
    public record GetOnlineIdRequest(string Username, string Password, string Price, string Desc, string ReqId);
    public record GetOnlineIdkindRequest(string Username, string Password, string Price, string Desc, string ReqId, string Kind);
    public record GetOnlineIdWithSettleRequest(string Username, string Password, string Price, string Desc, string ReqId, string Kind);
    public record GetOnlineStatusRequest(string Username, string Password, string OnlineId);
    public record ReqRefundRequest(string Username, string Password, string OnlineId, string RefundAmount);
    public record ReqReversalRequest(string Username, string Password, string OnlineId);
    public record ReqSettleOnlineRequest(string Username, string Password, string OnlineId);

    public static class PaymentRequestMapper
    {
        public static GetOnlineIdRequest ToGetOnlineIdRequest(string username, string password, string price, string desc, string reqId)
            => new(username, password, price, desc, reqId);

        public static GetOnlineIdkindRequest ToGetOnlineIdkindRequest(string username, string password, string price, string desc, string reqId, string kind)
            => new(username, password, price, desc, reqId, kind);

        public static GetOnlineIdWithSettleRequest ToGetOnlineIdWithSettleRequest(string username, string password, string price, string desc, string reqId, string kind)
            => new(username, password, price, desc, reqId, kind);

        public static GetOnlineStatusRequest ToGetOnlineStatusRequest(string username, string password, string onlineId)
            => new(username, password, onlineId);

        public static ReqRefundRequest ToReqRefundRequest(string username, string password, string onlineId, string refundAmount)
            => new(username, password, onlineId, refundAmount);

        public static ReqReversalRequest ToReqReversalRequest(string username, string password, string onlineId)
            => new(username, password, onlineId);

        public static ReqSettleOnlineRequest ToReqSettleOnlineRequest(string username, string password, string onlineId)
            => new(username, password, onlineId);
    }
}



