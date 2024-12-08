using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.StoreProceduresModels
{
    public class StoreProcedureName
    {
        public const string InsertOnlinePay = "dbo.Sp_InsertOnlinePay";

        public const string SelectBankId = "dbo.Sp_SelectBnakID";

        public const string SelectSiteAuthentication = "dbo.Sp_SelectSiteAuthentication";

        public const string SelectOnlinePay = "dbo.Sp_SelectOnlinePay";

        public const string SelectBankDetail = "dbo.Sp_SelectBankDetail @Online_ID";

        public const string SelectOnlinePayDetail = "dbo.Sp_SelectOnlinePayDetail";

        public const string UpdateOnlinePayReversal = "dbo.Sp_UpdateOnlinePayReversal";

        public const string UpdateOnlinePayRefund = "dbo.Sp_UpdateOnlinePayRefund";

        public const string UpdateOnlinePayResWithSettle = "dbo.Sp_UpdateOnlinePayResWithSettle";

        public const string SelectPaymentDetail = "dbo.Sp_SelectPaymentDetail";

        public const string UpdateOnlinePayFailed = "dbo.Sp_UpdateOnlinePayFailed";

        public const string UpdateOnlinePay = "dbo.Sp_UpdateOnlinePay";

        public const string insertSiteError = "dbo.sp_insertSiteError";

        public const string UpdateOnlinePaySettleFailed = "dbo.Sp_UpdateOnlinePaySettleFailed";

        public const string UpdateOnlinePayment = "dbo.Sp_UpdateOnlinePayment";

        public const string UpdateOnlinePaymentFailed = "dbo.Sp_UpdateOnlinePaymentFailed";

        public const string InsertTransactionResult = "dbo.Sp_InsertTransactionResult";
    }
}
