using BamdadPaymentCore.Domain.Database;
using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.StoreProceduresModels;
using BamdadPaymentCore.Domain.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.StoreProceduresModels.Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BamdadPaymentCore.Domain.Repositories
{
    internal class BamdadPaymentRepository(NimkatOnlineContext context) : IBamdadPaymentRepository
    {
        public InsertOnlinePayResult InsertOnlinePay(InsertOnlinePayParameter parameter)
        {
            var bankIdParam = new SqlParameter("@Bank_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Bank_ID };

            var siteIdParam = new SqlParameter("@Site_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Site_ID };

            var priceParam = new SqlParameter("@Online_Price", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_Price };

            var descParam = new SqlParameter("@Online_Desc", SqlDbType.VarChar)
            { Direction = ParameterDirection.Input, Value = parameter.Online_Desc };

            var reqIdParam = new SqlParameter("@Online_ReqID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ReqID };

            var kindParam = new SqlParameter("@Online_Kind", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_Kind };

            var settleParam = new SqlParameter("@IsSettle", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.IsSettle };

            var typeParam = new SqlParameter("@Online_Type", SqlDbType.VarChar)
            { Direction = ParameterDirection.Input, Value = parameter.Online_Type };

            return context.Database.SqlQuery<InsertOnlinePayResult>($"EXEC {StoreProcedureName.InsertOnlinePay} {bankIdParam}, {siteIdParam}, {priceParam}, {descParam}, {reqIdParam}, {kindParam}, {settleParam}, {typeParam}")
            .ToList()
            .FirstOrDefault();
        }

        public List<SelectBankDetailResult> SelectBankDetail(SelectBankDetailParameter parameter)
        {
            var siteIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ID };

            return context.Database.SqlQuery<SelectBankDetailResult>($"EXEC {StoreProcedureName.SelectBankDetail}  {siteIdParam}").ToList();
        }

        public SelectBankIdResult SelectBankID(SelectBankIdParameter parameter)
        {

            var siteIdParam = new SqlParameter("@Site_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Site_ID };

            return context.Database.SqlQuery<SelectBankIdResult>($"EXEC {StoreProcedureName.SelectBankId}  {siteIdParam}")
                                   .ToList().FirstOrDefault();
        }

        public List<SelectOnlinePayResult> SelectOnlinePay(SelectOnlinePayParameter parameter)
        {
            var siteIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ID };

            return context.Database.SqlQuery<SelectOnlinePayResult>($"EXEC  {StoreProcedureName.SelectOnlinePay}  {siteIdParam}")
                                    .ToList();
        }

        public List<SelectOnlinePayDetailResult> SelectOnlinePayDetail(SelectOnlinePayDetailParameter parameter)
        {
            var siteIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ID };

            return context.Database.SqlQuery<SelectOnlinePayDetailResult>($"EXEC {StoreProcedureName.SelectOnlinePayDetail}  {siteIdParam}").ToList();
        }

        public SiteAuthenticationResult SelectSiteAuthentication(SiteAuthenticationParameter parameter)
        {
            var site_UserName = new SqlParameter("@Site_UserName", SqlDbType.VarChar)
            { Direction = ParameterDirection.Input, Value = parameter.Site_UserName };

            var site_Pass = new SqlParameter("@Site_Pass", SqlDbType.VarChar)
            { Direction = ParameterDirection.Input, Value = parameter.Site_Pass };

            var ipParam = new SqlParameter("@Site_IP", SqlDbType.VarChar)
            { Direction = ParameterDirection.Input, Value = parameter.Site_IP };

            return context.Database.SqlQuery<SiteAuthenticationResult>($"EXEC {StoreProcedureName.SelectSiteAuthentication} {site_UserName},{site_Pass},{ipParam}")
                                   .ToList().FirstOrDefault();
        }

        public UpdateOnlinePayRefundResult UpdateOnlinePayRefund(UpdateOnlinePayRefundParameter parameter)
        {
            var siteIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ID };

            var online_ErrorCodeParam = new SqlParameter("@Online_ErrorCode", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ErrorCode };

            return context.Database.SqlQuery<UpdateOnlinePayRefundResult>
                ($"EXEC {StoreProcedureName.UpdateOnlinePayRefund}  {siteIdParam},{online_ErrorCodeParam}")
                                   .ToList().FirstOrDefault();
        }

        public UpdateOnlinePayReversalResult UpdateOnlinePayReversal(UpdateOnlinePayReversalParameter parameter)
        {
            var siteIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ID };

            var online_ErrorCodeParam = new SqlParameter("@Online_ErrorCode", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ErrorCode };

            return context.Database.SqlQuery<UpdateOnlinePayReversalResult>
                ($"EXEC {StoreProcedureName.UpdateOnlinePayReversal}  {siteIdParam},{online_ErrorCodeParam}")
                                   .ToList().FirstOrDefault();
        }

        public UpdateOnlinePaySettleFailedResult UpdateOnlinePaySettleFailed(UpdateOnlinePayResWithSettleFailedParameter parameter)
        {
            var siteIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ID };

            var errorCodeParam = new SqlParameter("@Online_ErrorCode", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ErrorCode };

            return context.Database.SqlQuery<UpdateOnlinePaySettleFailedResult>($"EXEC {StoreProcedureName.UpdateOnlinePaySettleFailed}  {siteIdParam},{errorCodeParam}")
                .ToList()
                .FirstOrDefault();
        }

        public void UpdateOnlinePayResWithSettle(UpdateOnlinePayResWithSettleParameter parameter)
        {
            var onlineId = new SqlParameter("@Online_ID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_ID
            };

            context.Database.SqlQuery<UpdateOnlinePayResWithSettleResult>($"EXEC {StoreProcedureName.UpdateOnlinePayResWithSettle}  {onlineId}")
                .ToList()
                .FirstOrDefault();
        }

        public SelectPaymentDetailResult SelectPaymentDetail(SelectPaymentDetailParameter parameter)
        {
            var siteIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ID };

            return context.Database.SqlQuery<SelectPaymentDetailResult>($"EXEC {StoreProcedureName.SelectPaymentDetail}  {siteIdParam}").ToList().FirstOrDefault();
        }

        public UpdateOnlinePayResult UpdateOnlinePay(UpdateOnlinePayParameter parameter)
        {
            var onlineIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_ID
            };

            var transactionNoParam = new SqlParameter("@Online_TransactionNo", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_TransactionNo
            };

            var orderNoParam = new SqlParameter("@Online_OrderNo", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_OrderNo
            };

            var errorCodeParam = new SqlParameter("@Online_ErrorCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_ErrorCode
            };

            var cardHolderInfoParam = new SqlParameter("@CardHolderInfo", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.CardHolderInfo
            };
            return context.Database.SqlQuery<UpdateOnlinePayResult>($"EXEC {StoreProcedureName.UpdateOnlinePayFailed}  {onlineIdParam},{transactionNoParam},{orderNoParam},{errorCodeParam},{cardHolderInfoParam}").ToList().FirstOrDefault();
        }

        public UpdateOnlinePayFailedResult UpdateOnlinePayFailed(UpdateOnlinePayFailedParameter parameter)
        {
            var onlineIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_ID
            };

            var transactionNoParam = new SqlParameter("@Online_TransactionNo", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_TransactionNo
            };

            var orderNoParam = new SqlParameter("@Online_OrderNo", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_OrderNo
            };

            var errorCodeParam = new SqlParameter("@Online_ErrorCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_ErrorCode
            };

            var cardHolderInfoParam = new SqlParameter("@CardHolderInfo", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.CardHolderInfo
            };
            return context.Database.SqlQuery<UpdateOnlinePayFailedResult>($"EXEC {StoreProcedureName.UpdateOnlinePay}  {onlineIdParam},{transactionNoParam},{orderNoParam},{errorCodeParam},{cardHolderInfoParam}").ToList().FirstOrDefault();
        }

        public void insertSiteError(insertSiteErrorParameter parameter)
        {
            var siteIdParam = new SqlParameter("@ErrorMessage", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.ErrorMessage };

            var errorCodeParam = new SqlParameter("@ErrorSource", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.ErrorSource };

            context.Database.SqlQuery<object>($"EXEC {StoreProcedureName.insertSiteError}  {siteIdParam},{errorCodeParam}")
                .ToList()
                .FirstOrDefault();
        }

        public UpdateOnlinePayWithSettleResult UpdateOnlinePayWithSettle(UpdateOnlinePayWithSettleParameter parameter)
        {
            var onlineIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_ID
            };

            var transactionNoParam = new SqlParameter("@Online_TransactionNo", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_TransactionNo
            };

            var orderNoParam = new SqlParameter("@Online_OrderNo", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_OrderNo
            };

            var errorCodeParam = new SqlParameter("@Online_ErrorCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_ErrorCode
            };

            var cardHolderInfoParam = new SqlParameter("@CardHolderInfo", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.CardHolderInfo
            };
            return context.Database.SqlQuery<UpdateOnlinePayWithSettleResult>($"EXEC {StoreProcedureName.UpdateOnlinePayFailed}  {onlineIdParam},{transactionNoParam},{orderNoParam},{errorCodeParam},{cardHolderInfoParam}").ToList().FirstOrDefault();
        }
    }
}

