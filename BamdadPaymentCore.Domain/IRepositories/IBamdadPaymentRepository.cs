﻿using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.Models.ControllerDto;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;


namespace BamdadPaymentCore.Domain.IRepositories
{
    public interface IBamdadPaymentRepository
    {
        public UpdateIsVerifyResult UpdateIsVerify(UpdateIsVerifyParameter parameter);

        public Task<List<GetFailedSettlePaymentsResult>> GetFailedInSettleBankPayments(GetFailedVerifyPaymentsParameter parameter);

        public void Update(OnlinePay onlinePay);

        InsertIntoOnlinePayResult InsertOnlinePay(InsertIntoOnlinePayParameter parameter);

        SelectBankIdResult SelectBankID(SelectBankIdParameter parameter);

        SiteAuthenticationResult SelectSiteAuthentication(SiteAuthenticationParameter parameter);

        SelectOnlinePayResult SelectOnlinePay(SelectOnlinePayParameter parameter);

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

        public List<GetPaymentReportResponse> PaymentReport(GetPaymentReportParameter parameter);

        public int? GetSiteId(GetSiteIdParameter parameter);

        public UpdateTransactionResultSp UpdateTransactionResult(UpdateTransactionResultParameter parameter);

        public InsertTransactionResultErrorResult InsertTransactionResultError(InsertTransactionResultErrorParameter parameter);

        public UpdateIsSettleResult UpdateIsSettle(UpdateIsSettleParameter parameter);

        public Task<List<GetFailedVerifyPaymentsResult>> GetFailedVerifyPayments(GetFailedVerifyPaymentsParameter parameter);

        public UpdateVerifyFailedPaymentResult UpdateVerifyFailedPayment(UpdateVerifyFailedPaymentParameter parameter);
    }
}
