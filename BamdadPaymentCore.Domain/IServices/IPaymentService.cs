using BamdadPaymentCore.Domain.Models.ControllerDto;
using BamdadPaymentCore.Domain.Models.SoapDto.Requests;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;

using System.Data;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;


namespace BamdadPaymentCore.Domain.IServices
{
    public interface IPaymentService
    {
        string GetOnlineIdDifferentTypes(string userName, string password, string onlinePrice, string desc, string reqId, string kind, bool autoSettle, string onlineType
            , string? mobleNomber = null);

        ApiResult<string> GetOnlineId(GetOnlineIdRequest request);

        bool RequestReversal(string username, string pass, string onlineId);

        DataTable RequestMellatSettle(SettleOnlineRequest request);

        bool ReqVerify(VerifyRequest request);

        string CancelPayment(string onlineId);

        string FreePayment(string onlineId);

        ApiResult<SelectOnlinePayResult> OnlineStatus(GetOnlineStatusParameter request);

        DataTable GetOnlineStatus(GetOnlineStatusParameter request);

        bool ReqRefund(ReqRefundRequest request);

        ApiResult<SelectOnlinePayResult> SettleAsan(ReqSettleOnlineRequest request);

        DataTable ReqSettleOnline(ReqSettleOnlineRequest request);

        void InsertSiteError(InsertSiteErrorParameter request);

        string UpdateOnlinePayFailed(string referenceNumber, string onlineId, string transactionNo, string orderNo, string errorCode, string cardHolderInfo);
    }
}
