using BamdadPaymentCore.Domain.Database;
using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.Enums;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.Models.ControllerDto;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Data;
using XAct;

namespace BamdadPaymentCore.Domain.Repositories
{
    internal class BamdadPaymentRepository(NimkatOnlineContext context) : IBamdadPaymentRepository
    {
        public void Update(OnlinePay onlinePay)
        {
            SqlParameter[] parameters =
            [
                new("@OnlineId", onlinePay.OnlineId),
                new("@OnlineTransactionNo", (object?)onlinePay.OnlineTransactionNo ?? DBNull.Value),
                new("@OnlineOrderNo", (object?)onlinePay.OnlineOrderNo ?? DBNull.Value),
                new("@OnlinePrice", (object?)onlinePay.OnlinePrice ?? DBNull.Value),
                new("@OnlineErrorCode", (object?)onlinePay.OnlineErrorCode ?? DBNull.Value),
                new("@IsSettle", (object?)onlinePay.IsSettle ?? DBNull.Value),
                new("@CardHolderInfo", (object?)onlinePay.CardHolderInfo ?? DBNull.Value),
                new("@ReferenceNumber", (object?)onlinePay.ReferenceNumber ?? DBNull.Value),
                new("@AutoSettle", (object?)onlinePay.AutoSettle ?? DBNull.Value)
            ];

            string query = $"EXEC {StoreProcedureName.UpdateOP} " +
               "@OnlineId, @OnlineTransactionNo, @OnlineOrderNo, @OnlinePrice, @OnlineErrorCode, " +
               "@IsSettle, @CardHolderInfo, @ReferenceNumber, @AutoSettle";


            context.Database.ExecuteSqlRaw(query, parameters);
        }

        public InsertIntoOnlinePayResult InsertOnlinePay(InsertIntoOnlinePayParameter parameter)
        {
            var bankId = new SqlParameter("@Bank_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Bank_ID };

            var siteId = new SqlParameter("@Site_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Site_ID };

            var price = new SqlParameter("@Online_Price", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_Price };

            var desc = new SqlParameter("@Online_Desc", SqlDbType.VarChar)
            { Direction = ParameterDirection.Input, Value = parameter.Online_Desc };

            var reqId = new SqlParameter("@Online_ReqID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ReqID };

            var kind = new SqlParameter("@Online_Kind", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_Kind };

            var autosettle = new SqlParameter("@AutoSettle", SqlDbType.Bit)
            { Direction = ParameterDirection.Input, Value = parameter.AutoSettle };

            var type = new SqlParameter("@Online_Type", SqlDbType.VarChar)
            { Direction = ParameterDirection.Input, Value = parameter.Online_Type };

            var mobileNomber = new SqlParameter("@MobileNomber", SqlDbType.VarChar)
            { Direction = ParameterDirection.Input, Value = parameter.MobileNomber ?? (object)DBNull.Value };

            return context.Database.SqlQuery<InsertIntoOnlinePayResult>(
                    $"EXEC {StoreProcedureName.InsertOnlinePayment} {bankId}, {siteId}, {price}, {desc}, {reqId}, {kind}, {autosettle}, {type}, {mobileNomber}")
                .ToList().FirstOrDefault()!;
        }

        public List<SelectBankDetailResult> SelectBankDetail(SelectBankDetailParameter parameter)
        {
            var siteIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ID };

            return context.Database
                .SqlQuery<SelectBankDetailResult>($"EXEC {StoreProcedureName.SelectBankDetail}  {siteIdParam}")
                .ToList();
        }

        public SelectBankIdResult SelectBankID(SelectBankIdParameter parameter)
        {
            var siteIdParam = new SqlParameter("@Site_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Site_ID };

            return context.Database
                .SqlQuery<SelectBankIdResult>($"EXEC {StoreProcedureName.SelectBankId}  {siteIdParam}")
                .ToList().FirstOrDefault();
        }

        public SelectOnlinePayResult SelectOnlinePay(SelectOnlinePayParameter parameter)
        {
            var siteIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ID };
            var d = context.Database
                .SqlQuery<SelectOnlinePayResult>($"EXEC  {StoreProcedureName.SelectOnlinePay}  {siteIdParam}")
                .ToList().First();
            return d;
        }

        public List<SelectOnlinePayDetailResult> SelectOnlinePayDetail(SelectOnlinePayDetailParameter parameter)
        {
            var siteIdParam = new SqlParameter("@Online_ID", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = parameter.Online_ID };

            return context.Database
                .SqlQuery<SelectOnlinePayDetailResult>(
                    $"EXEC dbo.Sp_SelectOnlinePayDetail @Online_ID = {parameter.Online_ID}").ToList();
        }

        public SiteAuthenticationResult SelectSiteAuthentication(SiteAuthenticationParameter parameter)
        {

            var site_UserName = new SqlParameter("@Site_UserName", SqlDbType.VarChar)
            { Direction = ParameterDirection.Input, Value = parameter.Site_UserName };

            var site_Pass = new SqlParameter("@Site_Pass", SqlDbType.VarChar)
            { Direction = ParameterDirection.Input, Value = parameter.Site_Pass };

            var ipParam = new SqlParameter("@Site_IP", SqlDbType.VarChar)
            { Direction = ParameterDirection.Input, Value = parameter.Site_IP };

            return context.Database.SqlQuery<SiteAuthenticationResult>
                    ($"EXEC {StoreProcedureName.SelectSiteAuthentication} {site_UserName},{site_Pass},{ipParam}")
                .ToList().FirstOrDefault()!;
        }

        public void UpdateOnlinePayRefund(UpdateOnlinePayRefundParameter parameter)
        {
            var siteIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ID };

            var online_ErrorCodeParam = new SqlParameter("@Online_ErrorCode", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ErrorCode };

            context.Database.SqlQuery<int>
                    ($"EXEC {StoreProcedureName.UpdateOnlinePayRefund}  {siteIdParam},{online_ErrorCodeParam}")
                .ToList().FirstOrDefault();
        }

        public void UpdateOnlinePayReversal(UpdateOnlinePayReversalParameter parameter)
        {
            var siteIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ID };

            var online_ErrorCodeParam = new SqlParameter("@Online_ErrorCode", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ErrorCode };

            context.Database.SqlQuery<int>
                    ($"EXEC {StoreProcedureName.UpdateOnlinePayReversal}  {siteIdParam},{online_ErrorCodeParam}")
                .ToList().FirstOrDefault();
        }

        public void UpdateOnlinePaySettleFailed(UpdateOnlinePayResWithSettleFailedParameter parameter)
        {
            var siteIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ID };

            var errorCodeParam = new SqlParameter("@Online_ErrorCode", SqlDbType.Int)
            { Direction = ParameterDirection.Input, Value = parameter.Online_ErrorCode };

            context.Database.SqlQuery<int>(
                    $"EXEC {StoreProcedureName.UpdateOnlinePaySettleFailed}  {siteIdParam},{errorCodeParam}")
                .ToList().FirstOrDefault();
        }

        public void UpdateOnlinePayResWithSettle(UpdateOnlinePayResWithSettleParameter parameter)
        {
            var onlineId = new SqlParameter("@Online_ID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_ID
            };

            context.Database.SqlQuery<int>($"EXEC {StoreProcedureName.UpdateOnlinePayResWithSettle}  {onlineId}")
                .ToList()
                .FirstOrDefault();
        }

        public SelectPaymentDetailResult SelectPaymentDetail(SelectPaymentDetailParameter parameter)
        {
            var siteIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)

            { Direction = ParameterDirection.Input, Value = parameter.Online_ID };

            return context.Database
                .SqlQuery<SelectPaymentDetailResult>($"EXEC {StoreProcedureName.SelectPaymentDetail}  {siteIdParam}")
                .AsEnumerable().FirstOrDefault();
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

            var referenceNumberParam = new SqlParameter("@ReferenceNumber", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.ReferenceNumber ?? (object)DBNull.Value,
            };

            return context.Database
                .SqlQuery<UpdateOnlinePayResult>(
                    $"EXEC {StoreProcedureName.UpdateOnlinePayFailed}  {onlineIdParam},{transactionNoParam},{orderNoParam},{errorCodeParam},{cardHolderInfoParam},{referenceNumberParam}")
                .ToList().FirstOrDefault();
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

            var referenceNumberParam = new SqlParameter("@ReferenceNumber", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.ReferenceNumber ?? (object)DBNull.Value,
            };

            return context.Database
                .SqlQuery<UpdateOnlinePayFailedResult>(
                    $"EXEC {StoreProcedureName.UpdateOnlinePayFailed}  {onlineIdParam},{transactionNoParam},{orderNoParam},{errorCodeParam},{cardHolderInfoParam},{referenceNumberParam}")
                .ToList().FirstOrDefault()!;
        }

        public void insertSiteError(InsertSiteErrorParameter parameter)
        {
            var siteIdParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar)
            { Direction = ParameterDirection.Input, Value = parameter.ErrorMessage };

            var errorCodeParam = new SqlParameter("@ErrorSource", SqlDbType.NVarChar)
            { Direction = ParameterDirection.Input, Value = parameter.ErrorSource };

            context.Database.SqlQuery<int>($"EXEC {StoreProcedureName.insertSiteError}  {siteIdParam},{errorCodeParam}")
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
            return context.Database
                .SqlQuery<UpdateOnlinePayWithSettleResult>(
                    $"EXEC {StoreProcedureName.UpdateOnlinePayFailed}  {onlineIdParam},{transactionNoParam},{orderNoParam},{errorCodeParam},{cardHolderInfoParam}")
                .ToList().FirstOrDefault();
        }

        public UpdateOnlinePayResult UpdateOnlinePayment(UpdateOnlinePayParameter parameter)
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

            var referenceNumberParam = new SqlParameter("@ReferenceNumber", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.ReferenceNumber ?? (object)DBNull.Value,
                IsNullable = true
            };

            return context.Database
                .SqlQuery<UpdateOnlinePayResult>(
                    $"EXEC {StoreProcedureName.UpdateOnlinePayment}  {onlineIdParam},{transactionNoParam},{orderNoParam},{errorCodeParam},{cardHolderInfoParam},{referenceNumberParam}")
                .ToList().FirstOrDefault();
        }

        public UpdateOnlinePayFailedResult UpdateOnlinePaymentFailed(UpdateOnlinePayFailedParameter parameter)
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
                Value = parameter.Online_OrderNo ?? (object)DBNull.Value,
            };

            var errorCodeParam = new SqlParameter("@Online_ErrorCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_ErrorCode
            };

            var cardHolderInfoParam = new SqlParameter("@CardHolderInfo", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.CardHolderInfo ?? (object)DBNull.Value,
            };

            var referenceNumberParam = new SqlParameter("@ReferenceNumber", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.ReferenceNumber ?? (object)DBNull.Value,
                IsNullable = true,
            };

            return context.Database
                .SqlQuery<UpdateOnlinePayFailedResult>(
                    $"EXEC {StoreProcedureName.UpdateOnlinePaymentFailed}  {onlineIdParam},{transactionNoParam},{orderNoParam},{errorCodeParam},{cardHolderInfoParam},{referenceNumberParam}")
                .ToList().FirstOrDefault();
        }

        public int InsertTransactionResult(InsertTransactionResultParameter parameter)
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

            var referenceNumberParam = new SqlParameter("@ReferenceNumber", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.ReferenceNumber ?? (object)DBNull.Value,
                IsNullable = true
            };

            return context.Database
                .SqlQuery<int>($"EXEC {StoreProcedureName.InsertTransactionResult}  {onlineIdParam},{transactionNoParam},{orderNoParam},{errorCodeParam},{referenceNumberParam},{cardHolderInfoParam}")
                .ToList().FirstOrDefault();
        }

        public List<GetPaymentReportResponse> PaymentReport(GetPaymentReportParameter parameter)
          => context.Database.SqlQueryRaw<GetPaymentReportResponse>($"EXEC {StoreProcedureName.GetPaymentReport} @StartDate,@EndDate,@Status,@SiteId", parameter.Parameters)
            .ToList();

        public int? GetSiteId(GetSiteIdParameter parameter)
        => context.Database.SqlQueryRaw<int?>($"EXEC {StoreProcedureName.GetSiteId} @UserName,@Password", parameter.GetSiteIdSqlParameter)
                .ToList().FirstOrDefault();

        public UpdateTransactionResultSp UpdateTransactionResult(UpdateTransactionResultParameter parameter)
        {
            var onlineIdParam = new SqlParameter("@Online_ID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_ID
            };
            var errorCode = parameter.ErrorCode.TryParseAsInt(-2);
            var ErrorCodeParam = new SqlParameter("@ErrorCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = errorCode
            };
            var transactionNoParam = new SqlParameter("@Online_TransactionNo", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_TransactionNo
            };

            var orderNoParam = new SqlParameter("@Online_OrderNo", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.Online_OrderNo ?? (object)DBNull.Value,
            };

            var cardHolderInfoParam = new SqlParameter("@CardHolderInfo", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.CardHolderInfo ?? (object)DBNull.Value,
            };

            var referenceNumberParam = new SqlParameter("@ReferenceNumber", SqlDbType.VarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.ReferenceNumber ?? (object)DBNull.Value,
                IsNullable = true,
            };

            return context.Database
                .SqlQuery<UpdateTransactionResultSp>(
                    $"EXEC {StoreProcedureName.UpdateTransactionResult}  {onlineIdParam},{ErrorCodeParam},{transactionNoParam},{orderNoParam},{cardHolderInfoParam},{referenceNumberParam}")
                .ToList().FirstOrDefault();
        }

        public InsertTransactionResultErrorResult InsertTransactionResultError(InsertTransactionResultErrorParameter parameter)
        {
            SqlParameter[] parameters =
            [
                new("@ResultCode", parameter.ResultCode ?? (object)DBNull.Value),
                new("@ErrorMessage", parameter.ErrorMessage ?? (object)DBNull.Value),
                new("@OnlineId", parameter.OnlineId ?? (object)DBNull.Value),
                new("@Bank_MerchantID", parameter.Bank_MerchantID ?? (object)DBNull.Value),
                new("@Bank_User", parameter.Bank_User ?? (object)DBNull.Value),
                new("@Bank_Pass", parameter.Bank_Pass ?? (object)DBNull.Value),
                new("@BankCode", parameter.BankCode ?? (object)DBNull.Value),
                new("@Online_Status", parameter.Online_Status ?? (object)DBNull.Value),
                new("@Online_Kind", parameter.Online_Kind ?? (object)DBNull.Value),
                new("@IsSettle", parameter.IsSettle ?? (object)DBNull.Value),
                new("@AutoSettle", parameter.AutoSettle ?? (object)DBNull.Value),
                new("@MobileNomber", parameter.MobileNomber ?? (object)DBNull.Value)
            ];

            string query = $"EXEC {StoreProcedureName.InsertTransactionResultError} @ResultCode, @ErrorMessage, @OnlineId, @Bank_MerchantID, @Bank_User, @Bank_Pass, @BankCode, @Online_Status, @Online_Kind, @IsSettle, @AutoSettle, @MobileNomber";

            return context.Database.SqlQueryRaw<InsertTransactionResultErrorResult>(query, parameters).ToList().First();
        }

        public UpdateIsSettleResult UpdateIsSettle(UpdateIsSettleParameter parameter)
        {
            SqlParameter[] parameters =
            [
               new("@OnlineId", parameter.OnlineId),
                new("@IsSettle", parameter.IsSettle)
            ];

            string query = $"EXEC {StoreProcedureName.sp_UpdateIsSettle} @OnlineId, @IsSettle";

            return context.Database.SqlQueryRaw<UpdateIsSettleResult>(query, parameters).ToList().First();
        }

        public async Task<List<GetFailedVerifyPaymentsResult>> GetFailedVerifyPayments(GetFailedVerifyPaymentsParameter parameter)
        {
            var onlineIdParam = new SqlParameter("@ErrorCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = parameter.AsanError
            };
            return await context.Database.SqlQuery<GetFailedVerifyPaymentsResult>($"EXEC {StoreProcedureName.GetFailedVerifyPayments} {onlineIdParam}").ToListAsync();
        }

        public async Task<List<GetFailedSettlePaymentsResult>> GetFailedInSettleBankPayments(GetFailedVerifyPaymentsParameter parameter)
        {
            var IsSettle = new SqlParameter("@IsSettle", parameter.AsanError);

            return await context.Database.SqlQuery<GetFailedSettlePaymentsResult>($"EXEC {StoreProcedureName.GetFailedInSettleBankPayments} {IsSettle}").ToListAsync();
        }

        public UpdateVerifyFailedPaymentResult UpdateVerifyFailedPayment(UpdateVerifyFailedPaymentParameter parameter)
        {
            var ErrorCode = new SqlParameter("@ErrorCode", parameter.ErrorCode);

            var OnlineId = new SqlParameter("@OnlineId", parameter.OnlineId);

            return context.Database.SqlQuery<UpdateVerifyFailedPaymentResult>($"EXEC {StoreProcedureName.UpdateVerifyFailedPayment} {OnlineId},{ErrorCode}").ToList().First();
        }

        public UpdateIsVerifyResult UpdateIsVerify(UpdateIsVerifyParameter parameter)
        {
            var ErrorCode = new SqlParameter("@IsVerified", parameter.IsVerify);

            var OnlineId = new SqlParameter("@OnlineId", parameter.OnlineId);

            return context.Database.SqlQuery<UpdateIsVerifyResult>($"EXEC {StoreProcedureName.UpdateIsVerify} {ErrorCode},{OnlineId}").ToList().First();
        }
    }
}