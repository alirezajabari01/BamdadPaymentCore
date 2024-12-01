using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.SOAP
{
    public interface IPaymentSoapService
    {
        public string GetOnlineIdkind(string username, string pass, string price, string desc, string reqId, string kind);

        public string GetOnlineIdWithSettle(string username, string pass, string price, string desc, string reqId, string kind);

        public string GetOnlineId(string username, string pass, string price, string desc, string reqId);

        public DataTable GetOnlineStatus(string username, string pass, string onlineId);

        public DataTable ReqSettleOnline(string username, string pass, string onlineId);

        public bool ReqReversal(string username, string pass, string onlineId);

        public bool ReqRefund(string username, string pass, string onlineId, string refundAmount);
    }
}
