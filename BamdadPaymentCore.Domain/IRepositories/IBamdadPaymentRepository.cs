﻿using BamdadPaymentCore.Domain.Entites;
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
        InsertOnlinePayResult InsertOnlinePay(InsertOnlinePayParameter parameter);

        SelectBankIdResult SelectBankID(SelectBankIdParameter parameter);

        SiteAuthenticationResult SelectSiteAuthentication(SiteAuthenticationParameter parameter);

        List<SelectOnlinePayResult> SelectOnlinePay(SelectOnlinePayParameter parameter);

        List<SelectBankDetailResult> SelectBankDetail(SelectBankDetailParameter parameter);

        List<SelectOnlinePayDetailResult> SelectOnlinePayDetail(SelectOnlinePayDetailParameter parameter);

        UpdateOnlinePayReversalResult UpdateOnlinePayReversal(UpdateOnlinePayReversalParameter parameter);

        UpdateOnlinePayRefundResult UpdateOnlinePayRefund(UpdateOnlinePayRefundParameter parameter);

        UpdateOnlinePayResWithSettleResult UpdateOnlinePayResWithSettle(UpdateOnlinePayResWithSettleParameter parameter);

        UpdateOnlinePaySettleFailedResult UpdateOnlinePaySettleFailed(UpdateOnlinePayResWithSettleFailedParameter parameter);

        SelectPaymentDetailResult SelectPaymentDetail(SelectPaymentDetailParameter parameter);

        UpdateOnlinePayResult UpdateOnlinePay(UpdateOnlinePayParameter parameter);

        UpdateOnlinePayFailedResult UpdateOnlinePayFailed(UpdateOnlinePayFailedParameter parameter);

        void insertSiteError(insertSiteErrorParameter parameter);
    }
}
