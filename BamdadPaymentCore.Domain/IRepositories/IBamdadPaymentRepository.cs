using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.StoreProceduresModels;
using BamdadPaymentCore.Domain.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.StoreProceduresModels.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.IRepositories
{
    public interface IBamdadPaymentRepository
    {
        InsertIntoOnlinePayResult InsertOnlinePay(InsertIntoOnlinePayParameter parameter);

        SelectBankIdResult SelectBankID(SelectBankIdParameter parameter);

        SiteAuthenticationResult SelectSiteAuthentication(SiteAuthenticationParameter parameter);

        List<SelectOnlinePayResult> SelectOnlinePay(SelectOnlinePayParameter parameter);

        List<SelectBankDetailResult> SelectBankDetail(SelectBankDetailParameter parameter);

        List<SelectOnlinePayDetailResult> SelectOnlinePayDetail(SelectOnlinePayDetailParameter parameter);

        void UpdateOnlinePayReversal(UpdateOnlinePayReversalParameter parameter);

        void UpdateOnlinePayRefund(UpdateOnlinePayRefundParameter parameter);

        void UpdateOnlinePayResWithSettle(UpdateOnlinePayResWithSettleParameter parameter);

        void UpdateOnlinePaySettleFailed(UpdateOnlinePayResWithSettleFailedParameter parameter);

        SelectPaymentDetailResult SelectPaymentDetail(SelectPaymentDetailParameter parameter);

        UpdateOnlinePayResult UpdateOnlinePayment(UpdateOnlinePayParameter parameter);

        UpdateOnlinePayFailedResult UpdateOnlinePaymentFailed(UpdateOnlinePayFailedParameter parameter);

        void insertSiteError(InsertSiteErrorParameter parameter);

        UpdateOnlinePayWithSettleResult UpdateOnlinePayWithSettle(UpdateOnlinePayWithSettleParameter parameter);

        int InsertTransactionResult(InsertTransactionResultParameter parameter);
    }
}
