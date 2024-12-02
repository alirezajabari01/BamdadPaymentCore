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
        void UpdateOnlinePayResWithSettle(string onlineId);

        string GetOnlineId(GetOnlineIdRequest request);

        string GetOnlineIdkind(GetOnlineIdkindRequest request);

        string GetOnlineIdWithSettle(GetOnlineIdWithSettleRequest request);

        DataTable GetOnlineStatus(GetOnlineStatusParameter request);

        bool ReqRefund(ReqRefundRequest request);

        bool ReqReversal(ReqReversalRequest request);

        DataTable ReqSettleOnline(ReqSettleOnlineRequest request);

        SelectPaymentDetailResult SelectPaymentDetail(SelectPaymentDetailParameter onlineId);

        string UpdateOnlinePay(string onlineId, string transactionNo, string orderNo, string errorCode, string cardHolderInfo = "");

        SelectBankDetailResult SelectBankDetail(SelectBankDetailParameter request);

        string UpdateOnlinePayFailed(string onlineId, string transactionNo, string orderNo, string errorCode, string cardHolderInfo = "");

        string UpdateOnlinePayWithSettle(UpdateOnlinePayWithSettleParameter parameter);

        void InsertSiteError(InsertSiteErrorParameter request);
    }
}
