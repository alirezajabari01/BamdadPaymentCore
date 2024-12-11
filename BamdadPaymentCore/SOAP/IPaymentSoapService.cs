using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.SOAP
{
    [ServiceContract(Namespace = "http://mftlearning.com/"), XmlSerializerFormat]
    public interface IPaymentSoapService
    {
        [OperationContract]
        public string GetOnlineIdkind(string username, string pass, string price, string desc, string reqId, string kind, string? mobleNomber = null);

        [OperationContract]
        public string GetOnlineIdWithSettle(string username, string pass, string price, string desc, string reqId, string kind, string? mobleNomber = null);

        [OperationContract]
        public string GetOnlineId(string username, string pass, string price, string desc, string reqId, string? mobleNomber = null);

        [OperationContract]
        public string normal(string username, string pass, string price, string desc, string reqId, string? mobleNomber = null);

        [OperationContract]
        public DataTable GetOnlineStatus(string username, string pass, string onlineId);

        [OperationContract]
        public DataTable ReqSettleOnline(string username, string pass, string onlineId);

        [OperationContract]
        public bool ReqReversal(string username, string pass, string onlineId);

        [OperationContract]
        public bool ReqRefund(string username, string pass, string onlineId, string refundAmount);

        [OperationContract]
        public bool ReqVerify(string username, string pass, string onlineId);
    }
}
