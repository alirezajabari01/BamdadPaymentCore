using BamdadPaymentCore.Domain.Models.SoapDto.Requests;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;

using System.Data;


namespace BamdadPaymentCore.Domain.IServices
{
    public interface IPaymentService
    {
        string GetOnlineIdDifferentTypes(string userName, string password, string onlinePrice, string desc, string reqId, string kind, bool autoSettle = false, string onlineType = "payment");

        bool RequestReversal(string username, string pass, string onlineId);

        DataTable RequestSettleOnline(SettleOnlineRequest request);

        bool ReqVerify(VerifyRequest request);

        string CancelPayment(string onlineId);

        string FreePayment(string onlineId);

        DataTable GetOnlineStatus(GetOnlineStatusParameter request);

        bool ReqRefund(ReqRefundRequest request);

        DataTable ReqSettleOnline(ReqSettleOnlineRequest request);

        void InsertSiteError(InsertSiteErrorParameter request);

        string UpdateOnlinePayFailed(string referenceNumber, string onlineId, string transactionNo, string orderNo, string errorCode, string cardHolderInfo);
    }
}
