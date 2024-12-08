using BamdadPaymentCore.Domain.SoapDto.Requests;
using BamdadPaymentCore.Domain.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.StoreProceduresModels.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.IServices
{
    public interface IPaymentService
    {
        string GetOnlineIdDifferentTypes(string userName, string password, string onlinePrice, string desc, string reqId, string kind, bool autoSettle = false, string onlineType = "payment");

        bool RequestReversal(string username, string pass, string onlineId);

        string ProcessAsanPardakhtPayment(string onlineId);

        DataTable RequestSettleOnline(SettleOnlineRequest request);

        bool ReqVerify(VerifyRequest request);

        string CancelPayment(string onlineId);

        string FreePayment(string onlineId);


        string GetOnlineId(GetOnlineIdRequest request);

        string GetOnlineIdkind(GetOnlineIdkindRequest request);

        string GetOnlineIdWithSettle(GetOnlineIdWithSettleRequest request);

        DataTable GetOnlineStatus(GetOnlineStatusParameter request);

        bool ReqRefund(ReqRefundRequest request);

        bool ReqReversal(ReqReversalRequest request);

        DataTable ReqSettleOnline(ReqSettleOnlineRequest request);

        void InsertSiteError(InsertSiteErrorParameter request);

        string UpdateOnlinePayFailed(string referenceNumber, string onlineId, string transactionNo, string orderNo, string errorCode, string cardHolderInfo);
    }
}
