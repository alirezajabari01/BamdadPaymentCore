using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;


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
